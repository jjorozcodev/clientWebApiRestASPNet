using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using myClientWebApi.Models;
using Newtonsoft.Json;

namespace myClientWebApi.Controllers
{
    public class ProductsController : Controller
    {

        private string BaseURL = "http://localhost:54041/";

        public async Task<ActionResult> Index()
        {
            List<Product> listProducts = new List<Product>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage resp = await client.GetAsync("api/products/");

                if (resp.IsSuccessStatusCode)
                {
                    var prodResp = resp.Content.ReadAsStringAsync().Result;
                    listProducts = JsonConvert.DeserializeObject<List<Product>>(prodResp);
                }

                return View(listProducts);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            //Complete all fields...
            product.CategoryID = 1;
            product.SupplierID = 1;
            product.Discontinued = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL + "/api/products/");
                var postTask = client.PostAsJsonAsync<Product>("products", product);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error: Please, contact the administrator.");
            return View(product);
        }
    }
}