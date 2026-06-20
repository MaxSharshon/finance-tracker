using System.Globalization;
using FinanceTracker.Contracts.Budgets;
using FinanceTracker.Contracts.Categories;
using FinanceTracker.Contracts.FinancialOperations;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FinanceTracker.UI.Components.Pages.Import;

public partial class Transactions
{
    private const long MaxFileSize = 1024 * 1024;

    private readonly CultureInfo _currencyCulture = CultureInfo.GetCultureInfo("uk-UA");
    private readonly CultureInfo _parseCulture = CultureInfo.InvariantCulture;

    private List<CategoryResponse> _categories = [];
    private List<BudgetResponse> _budgets = [];
    private List<ImportRow> _rows = [];
    private string _rawCsv = string.Empty;
    private string? _budgetId;
    private bool _isParsing;
    private bool _isImporting;
    private string? _errorMessage;
    private string? _successMessage;

    [Inject] private ICategoryService CategoryService { get; set; } = null!;
    [Inject] private IBudgetService BudgetService { get; set; } = null!;
    [Inject] private IFinancialOperationService FinancialOperationService { get; set; } = null!;
    [Inject] private INotificationRefreshService NotificationRefreshService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _categories = (await CategoryService.GetAllAsync()).ToList();
            _budgets = (await BudgetService.GetAllAsync()).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load import data: {ex.Message}";
        }
    }

    private async Task LoadFileAsync(InputFileChangeEventArgs args)
    {
        try
        {
            await using var stream = args.File.OpenReadStream(MaxFileSize);
            using var reader = new StreamReader(stream);
            _rawCsv = await reader.ReadToEndAsync();
            await ParseCsvAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to read CSV file: {ex.Message}";
        }
    }

    private async Task ParseCsvAsync()
    {
        try
        {
            _isParsing = true;
            _errorMessage = null;
            _successMessage = null;

            var parsedRows = ParseRows(_rawCsv).ToList();

            if (parsedRows.Count == 0)
            {
                _rows = [];
                _errorMessage = "CSV does not contain transactions.";
                return;
            }

            foreach (var row in parsedRows)
            {
                await ApplySuggestionAsync(row);
            }

            _rows = parsedRows;
            _successMessage = $"Preview generated: {_rows.Count} transaction(s).";
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to parse CSV: {ex.Message}";
        }
        finally
        {
            _isParsing = false;
        }
    }

    private async Task ImportAsync()
    {
        var rowsWithoutCategory = _rows.Where(row => string.IsNullOrWhiteSpace(row.CategoryId)).ToList();

        if (rowsWithoutCategory.Count > 0)
        {
            _errorMessage = "Select category for all rows before import.";
            return;
        }

        var budgetId = Guid.TryParse(_budgetId, out var parsedBudgetId)
            ? parsedBudgetId
            : (Guid?)null;

        try
        {
            _isImporting = true;
            _errorMessage = null;
            _successMessage = null;

            var imported = 0;

            foreach (var row in _rows)
            {
                if (!Guid.TryParse(row.CategoryId, out var categoryId))
                {
                    _errorMessage = $"Invalid category for row: {row.Description}.";
                    return;
                }

                var request = new FinancialOperationRequest(
                    categoryId,
                    budgetId,
                    row.Amount,
                    row.Date,
                    row.Description,
                    []);

                var response = await FinancialOperationService.AddAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var details = await response.Content.ReadAsStringAsync();
                    _errorMessage = string.IsNullOrWhiteSpace(details)
                        ? $"Import stopped. Failed to import row: {row.Description}."
                        : $"Import stopped. Failed to import row: {row.Description}. {details}";
                    return;
                }

                imported++;
            }

            _rows = [];
            _rawCsv = string.Empty;
            await NotificationRefreshService.RequestRefreshAsync();
            _successMessage = $"Imported {imported} transaction(s).";
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to import transactions: {ex.Message}";
        }
        finally
        {
            _isImporting = false;
        }
    }

    private async Task ApplySuggestionAsync(ImportRow row)
    {
        var suggestion = await CategoryService.SuggestAsync(new CategorySuggestionRequest(
            row.Description,
            row.SignedAmount,
            row.OperationType));

        if (suggestion is null)
        {
            row.SuggestionText = "No suggestion";
            return;
        }

        row.CategoryId = suggestion.CategoryId.ToString();
        row.SuggestionText = suggestion.MatchedKeyword is null
            ? $"{suggestion.CategoryName} ({suggestion.Confidence:P0})"
            : $"{suggestion.CategoryName} ({suggestion.Confidence:P0}, {suggestion.MatchedKeyword})";
    }

    private IEnumerable<ImportRow> ParseRows(string csv)
    {
        var lines = csv
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        if (lines.Count == 0)
        {
            return [];
        }

        var startIndex = IsHeader(lines[0]) ? 1 : 0;
        var rows = new List<ImportRow>();

        for (var index = startIndex; index < lines.Count; index++)
        {
            var columns = SplitCsvLine(lines[index]);

            if (columns.Count < 3)
            {
                throw new InvalidOperationException($"Line {index + 1} must contain date, description, amount.");
            }

            var date = ParseDate(columns[0], index + 1);
            var description = columns[1].Trim();
            var signedAmount = ParseAmount(columns[2], index + 1);
            var operationType = signedAmount >= 0 ? "Income" : "Expense";

            rows.Add(new ImportRow
            {
                Date = date,
                Description = description,
                SignedAmount = signedAmount,
                Amount = Math.Abs(signedAmount),
                OperationType = operationType
            });
        }

        return rows;
    }

    private static bool IsHeader(string line)
    {
        return line.Contains("date", StringComparison.OrdinalIgnoreCase) ||
               line.Contains("description", StringComparison.OrdinalIgnoreCase) ||
               line.Contains("amount", StringComparison.OrdinalIgnoreCase);
    }

    private static List<string> SplitCsvLine(string line)
    {
        var result = new List<string>();
        var current = new List<char>();
        var inQuotes = false;
        var delimiter = line.Contains(';') && !line.Contains(',')
            ? ';'
            : ',';

        foreach (var character in line)
        {
            if (character == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (character == delimiter && !inQuotes)
            {
                result.Add(new string(current.ToArray()));
                current.Clear();
                continue;
            }

            current.Add(character);
        }

        result.Add(new string(current.ToArray()));
        return result;
    }

    private DateTime ParseDate(string value, int lineNumber)
    {
        var formats = new[] { "yyyy-MM-dd", "dd.MM.yyyy", "MM/dd/yyyy" };

        if (DateTime.TryParseExact(value.Trim(), formats, _parseCulture, DateTimeStyles.None, out var date) ||
            DateTime.TryParse(value.Trim(), out date))
        {
            return date;
        }

        throw new InvalidOperationException($"Line {lineNumber} has invalid date.");
    }

    private decimal ParseAmount(string value, int lineNumber)
    {
        var normalized = value.Trim().Replace(" ", string.Empty);

        if (decimal.TryParse(normalized, NumberStyles.Number, _parseCulture, out var amount) ||
            decimal.TryParse(normalized.Replace(',', '.'), NumberStyles.Number, _parseCulture, out amount))
        {
            return amount;
        }

        throw new InvalidOperationException($"Line {lineNumber} has invalid amount.");
    }

    private IEnumerable<CategoryResponse> GetCategories(string operationType)
    {
        return _categories.Where(category =>
            string.Equals(category.OperationType, operationType, StringComparison.OrdinalIgnoreCase));
    }

    private string FormatSignedAmount(ImportRow row)
    {
        var sign = row.OperationType == "Income" ? "+" : "-";
        return $"{sign}{row.Amount.ToString("C", _currencyCulture)}";
    }

    private sealed class ImportRow
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal SignedAmount { get; set; }
        public decimal Amount { get; set; }
        public string OperationType { get; set; } = "Expense";
        public string? CategoryId { get; set; }
        public string SuggestionText { get; set; } = "Not processed";
    }
}
