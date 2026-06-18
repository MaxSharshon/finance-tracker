using FinanceTracker.Contracts.Tags;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class TagService(HttpClient client)
    : Service<TagRequest, TagResponse>(client, ENDPOINT), ITagService
{
    private const string ENDPOINT = "Tags";
}
