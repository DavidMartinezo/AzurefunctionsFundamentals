using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingCartList.Models;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopingCartList;

public class ShoppingCartApi
{
    private readonly ILogger<ShoppingCartApi> _logger;
    private static List<ShopingCartItem> shoppingCartItems = new();

    public ShoppingCartApi(ILogger<ShoppingCartApi> logger)
    {
        _logger = logger;
    }

    [Function("GetShoppingCartItems")]
    public async Task<HttpResponseData> GetShoppingCartItems(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shoppingCartItems")] HttpRequestData req)
    {

       
        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(shoppingCartItems);
        return response;
    }
    [Function("GetShoppingCartItemById")]
    public async Task<HttpResponseData> GetShoppingCartItemById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shoppingCartItem/{id}")] HttpRequestData req, string id)
    {

      
        var item = shoppingCartItems.FirstOrDefault(i => i.Id == id);
        if (item is null)
        {
            var notFound = req.CreateResponse(HttpStatusCode.NotFound);
            await notFound.WriteStringAsync("Item not found");
            return notFound;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(item);
        return response;
    }

    [Function("CreateShoppingCartItem")]
    public static async Task<HttpResponseData> CreateShoppingCartItem(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CreateShoppingCartItem")] HttpRequestData req)
    {

        //log.LogInformation("CreateShoppingCartItem");
        string requestData = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<ShopingCartItem>(requestData);
        if (data is null || string.IsNullOrWhiteSpace(data.ItemName))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Invalid object");
            return badRequest;
        }
        var item = new ShopingCartItem
        {
            ItemName = data!.ItemName,
            Category= data.Category
        };

        shoppingCartItems.Add(item);
        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(item);
        return response;
    }
    [Function("PutShoppingCartItem")]
    public async Task<HttpResponseData> PutShoppingCartItem(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "shoppingCartItem/{id}")] HttpRequestData req, string id)
    {

        //log.LogInformation($"Update item with id {id}");
        var item = shoppingCartItems.FirstOrDefault(i => i.Id == id);
        if (item is null)
        {
            var notFound = req.CreateResponse(HttpStatusCode.NotFound);
            await notFound.WriteStringAsync("item not found");
            return notFound;
        }
        string requestData = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<ShopingCartItem>(requestData);

        if (data is null)
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Invalid Update data.");
            return badRequest;
        }
        item.Collected = data.Collected;
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(item);
        return response;

    }
    [Function("DeleteShoppingCartItem")]
    public static async Task<HttpResponseData> DeleteShoppingCartItem(
     [HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "shoppingCartItem/{id}")] HttpRequestData req, string id)
    {

        //log.LogInformation($"Delete shopping cart Item with id {id}");
        var item = shoppingCartItems.FirstOrDefault(i => i.Id == id);
        if (item is null)
        {
            var notFound = req.CreateResponse(HttpStatusCode.NotFound);
            await notFound.WriteStringAsync("item not found");
            return notFound;
        }
        shoppingCartItems.Remove(item);
        var response = req.CreateResponse(HttpStatusCode.NoContent);

        return response;
    }
}