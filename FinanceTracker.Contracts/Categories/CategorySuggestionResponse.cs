namespace FinanceTracker.Contracts.Categories;

public record CategorySuggestionResponse(
    Guid CategoryId,
    string CategoryName,
    string OperationType,
    decimal Confidence,
    string? MatchedKeyword);
