using System.Net.Http.Json;
using System.Web;
using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;
    private const string RemoteServiceBaseUrl = "api/catalog/";

    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for API requests.</param>
    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets a catalog item by its ID.
    /// </summary>
    /// <param name="id">The item ID.</param>
    /// <returns>The catalog item or null if not found.</returns>
    public async Task<CatalogItem?> GetCatalogItemAsync(int id)
    {
        var uri = $"{RemoteServiceBaseUrl}items/{id}";
        return await _httpClient.GetFromJsonAsync<CatalogItem?>(uri);
    }

    /// <summary>
    /// Gets a paginated list of catalog items.
    /// </summary>
    /// <param name="pageIndex">The page index.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="brand">Optional brand filter.</param>
    /// <param name="type">Optional type filter.</param>
    /// <returns>The paginated catalog result.</returns>
    public async Task<CatalogResult> GetCatalogItemsAsync(int pageIndex, int pageSize, int? brand, int? type)
    {
        var uri = GetAllCatalogItemsUri(RemoteServiceBaseUrl, pageIndex, pageSize, brand, type);
        var result = await _httpClient.GetFromJsonAsync<CatalogResult>(uri);
        return result ?? new CatalogResult(0, 0, 0, new List<CatalogItem>());
    }

    /// <summary>
    /// Gets a list of catalog items by their IDs.
    /// </summary>
    /// <param name="ids">The item IDs.</param>
    /// <returns>The list of catalog items.</returns>
    public async Task<List<CatalogItem>> GetCatalogItemsAsync(IEnumerable<int> ids)
    {
        var uri = $"{RemoteServiceBaseUrl}items/by?ids={string.Join("&ids=", ids)}";
        var result = await _httpClient.GetFromJsonAsync<List<CatalogItem>>(uri);
        return result ?? new List<CatalogItem>();
    }

    /// <summary>
    /// Gets catalog items with semantic relevance to the given text.
    /// </summary>
    /// <param name="page">The page index.</param>
    /// <param name="take">The page size.</param>
    /// <param name="text">The search text.</param>
    /// <returns>The paginated catalog result.</returns>
    public async Task<CatalogResult> GetCatalogItemsWithSemanticRelevanceAsync(int page, int take, string text)
    {
        var url = $"{RemoteServiceBaseUrl}items/withsemanticrelevance?text={HttpUtility.UrlEncode(text)}&pageIndex={page}&pageSize={take}";
        var result = await _httpClient.GetFromJsonAsync<CatalogResult>(url);
        return result ?? new CatalogResult(0, 0, 0, new List<CatalogItem>());
    }

    /// <summary>
    /// Gets all catalog brands.
    /// </summary>
    /// <returns>The list of catalog brands.</returns>
    public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
    {
        var uri = $"{RemoteServiceBaseUrl}catalogBrands";
        var result = await _httpClient.GetFromJsonAsync<CatalogBrand[]>(uri);
        return result ?? Array.Empty<CatalogBrand>();
    }

    /// <summary>
    /// Gets all catalog item types.
    /// </summary>
    /// <returns>The list of catalog item types.</returns>
    public async Task<IEnumerable<CatalogItemType>> GetTypesAsync()
    {
        var uri = $"{RemoteServiceBaseUrl}catalogTypes";
        var result = await _httpClient.GetFromJsonAsync<CatalogItemType[]>(uri);
        return result ?? Array.Empty<CatalogItemType>();
    }

    private static string GetAllCatalogItemsUri(string baseUri, int pageIndex, int pageSize, int? brand, int? type)
    {
        string filterQs = string.Empty;
        if (type.HasValue)
        {
            filterQs += $"type={type.Value}&";
        }
        if (brand.HasValue)
        {
            filterQs += $"brand={brand.Value}&";
        }
        return $"{baseUri}items?{filterQs}pageIndex={pageIndex}&pageSize={pageSize}";
    }

    public Task<CatalogItem?> GetCatalogItem(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CatalogResult> GetCatalogItems(int pageIndex, int pageSize, int? brand, int? type)
    {
        throw new NotImplementedException();
    }

    public Task<List<CatalogItem>> GetCatalogItems(IEnumerable<int> ids)
    {
        throw new NotImplementedException();
    }

    public Task<CatalogResult> GetCatalogItemsWithSemanticRelevance(int page, int take, string text)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CatalogBrand>> GetBrands()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CatalogItemType>> GetTypes()
    {
        throw new NotImplementedException();
    }
}
