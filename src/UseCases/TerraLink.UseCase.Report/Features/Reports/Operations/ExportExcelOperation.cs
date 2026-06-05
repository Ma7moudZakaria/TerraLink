using ClosedXML.Excel;
using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Reflection;
using TerraLink.UseCase.Report.Features.Reports.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Report.Features.Reports.Operations;

public sealed partial class ExportExcelOperation(IRepository<LookupSetEntity> lookupSets, IServiceProvider services)
    : IOperationHandler<ExportExcelOperation.Request, ExportExcelOperation.Response>
{
    public async Task<ErrorOr<ExportExcelOperation.Response>> HandleAsync(ExportExcelOperation.Request request, CancellationToken ct = default)
    {
        LookupSetEntity? lookupSet = await lookupSets.GetAsync(
            new ReportLookupSetByIdSpec(request.LookupSetId),
            ct);

        if (lookupSet is null)
            return Error.Validation(ErrorCode.NoItemExist, "There is no report exist");

        string? entityCode = lookupSet.Descriptions.Get("en");
        if (string.IsNullOrWhiteSpace(entityCode))
            return Error.Validation(ErrorCode.NoItemEffected, "No entity found");

        Type? entityType = ResolveEntityTypeFromLookupCode(entityCode);
        if (entityType is null)
            return Error.Validation(ErrorCode.NoItemEffected, "No entity found");

        List<object> data = await ListByEntityAsync(entityType, ct);
        if (data.Count == 0)
            return Error.Validation(ErrorCode.NoItemEffected, "No data found for entity mapped");

        List<ActiveColumn> activeColumns = lookupSet.LookupItems
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .Select(x => new ActiveColumn(x.Code))
            .ToList();
        if (activeColumns.Count == 0)
            return Error.Validation(ErrorCode.NoItemCorrect, "No active columns configured");

        List<Dictionary<string, object?>> rows = [];
        foreach (var entity in data)
        {
            Dictionary<string, object?> row = [];
            foreach (var col in activeColumns)
            {
                string header = GetHeaderFromCode(col.Code);
                row[header] = GetNestedValue(entity, col.Code);
            }
            rows.Add(row);
        }

        var sheet = new ExcelSheetRequest<Dictionary<string, object?>> { SheetName = lookupSet.Code, Data = rows };
        byte[] fileBytes = ExportToExcel([sheet]);

        return new ExportExcelOperation.Response(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{lookupSet.Code}_{DateTime.UtcNow:yyyyMMdd_HHmm}.xlsx");
    }

    private static object? GetNestedValue(object src, string propertyPath)
    {
        if (src is null || string.IsNullOrWhiteSpace(propertyPath)) return null;
        object? current = src;
        foreach (var part in propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            if (current is null) return null;
            PropertyInfo? prop = current.GetType().GetProperty(part);
            if (prop is null) return null;
            current = prop.GetValue(current);
        }
        return current;
    }

    private sealed record ActiveColumn(string Code);

    private sealed class ExcelSheetRequest<TModel> where TModel : class
    {
        public string SheetName { get; init; } = "Sheet1";
        public IEnumerable<TModel> Data { get; init; } = Enumerable.Empty<TModel>();
        public string[]? ExcludedColumns { get; init; }
    }

    private static string GetHeaderFromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return string.Empty;
        string[] parts = code.Split('.', StringSplitOptions.RemoveEmptyEntries);
        return parts[^1];
    }

    private static Type? ResolveEntityTypeFromLookupCode(string code)
        => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(SafeGetTypes)
            .FirstOrDefault(t => t.Name.Equals(code, StringComparison.OrdinalIgnoreCase));

    private async Task<List<object>> ListByEntityAsync(Type entityType, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        if (entityType.ContainsGenericParameters)
        {
            throw new ArgumentException($"Type '{entityType}' is an open generic and cannot be used for export.", nameof(entityType));
        }

        Type repositoryType = typeof(IRepository<>).MakeGenericType(entityType);
        object repository = services.GetRequiredService(repositoryType);

        MethodInfo listMethod = repositoryType.GetMethod(nameof(IRepository<LookupSetEntity>.ListAsync), [typeof(CancellationToken)])
            ?? throw new MissingMethodException(repositoryType.FullName, nameof(IRepository<LookupSetEntity>.ListAsync));

        object task = listMethod.Invoke(repository, [ct])
            ?? throw new InvalidOperationException($"Repository for '{entityType.Name}' did not return a task.");

        await ((Task)task).ConfigureAwait(false);

        object? result = task.GetType().GetProperty(nameof(Task<object>.Result))?.GetValue(task);
        return result is IEnumerable enumerable
            ? enumerable.Cast<object>().ToList()
            : [];
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try { return assembly.GetTypes(); }
        catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t is not null)!; }
    }

    private static object? NormalizeExcelValue(object? value)
    {
        if (value is null) return null;
        if (value is Localized localized) return localized.Get("en");
        if (value is DateTime dt) return dt.ToString("yyyy-MM-dd");
        if (value.GetType().IsEnum) return value.ToString();
        if (value is string or int or long or double or decimal or bool) return value;
        return value.ToString();
    }

    private static byte[] ExportToExcel<TModel>(IEnumerable<ExcelSheetRequest<TModel>> sheets) where TModel : class
    {
        using XLWorkbook workbook = new();
        foreach (var sheet in sheets) AddWorksheet(workbook, sheet);
        using MemoryStream stream = new();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void AddWorksheet<TModel>(XLWorkbook workbook, ExcelSheetRequest<TModel> sheet) where TModel : class
    {
        IXLWorksheet worksheet = workbook.Worksheets.Add(string.IsNullOrWhiteSpace(sheet.SheetName) ? "Sheet1" : sheet.SheetName);
        if (sheet.Data is null || !sheet.Data.Any()) return;

        if (typeof(TModel) == typeof(Dictionary<string, object?>))
        {
            var rows = sheet.Data.Cast<Dictionary<string, object?>>().ToList();
            if (rows.Count == 0) return;

            var headers = rows[0].Keys.ToList();
            for (int col = 0; col < headers.Count; col++)
                worksheet.Cell(1, col + 1).SetValue(headers[col]).Style.Font.SetBold();

            int rowIndex = 2;
            foreach (var rowItem in rows)
            {
                for (int col = 0; col < headers.Count; col++)
                {
                    var value = rowItem.TryGetValue(headers[col], out var v) ? v : null;
                    worksheet.Cell(rowIndex, col + 1).SetValue(XLCellValue.FromObject(NormalizeExcelValue(value)));
                }
                rowIndex++;
            }
            worksheet.Columns().AdjustToContents();
            return;
        }

        var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0 && (sheet.ExcludedColumns is null || !sheet.ExcludedColumns.Contains(p.Name)))
            .ToList();
        if (properties.Count == 0) return;

        for (int col = 0; col < properties.Count; col++)
            worksheet.Cell(1, col + 1).SetValue(properties[col].Name).Style.Font.SetBold();

        int row = 2;
        foreach (var item in sheet.Data)
        {
            for (int col = 0; col < properties.Count; col++)
            {
                var rawValue = properties[col].GetValue(item);
                worksheet.Cell(row, col + 1).SetValue(XLCellValue.FromObject(rawValue));
            }
            row++;
        }
        worksheet.Columns().AdjustToContents();
    }
}
