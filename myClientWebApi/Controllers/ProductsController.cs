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
    }
}