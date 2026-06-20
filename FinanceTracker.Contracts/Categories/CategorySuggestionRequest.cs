namespace FinanceTracker.Contracts.Categories;

public record CategorySuggestionRequest(
    string Description,
    decimal Amount,
    string OperationType);
