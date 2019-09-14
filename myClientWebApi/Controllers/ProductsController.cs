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

        public ActionResult Edit(int id)
        {
            Product product = null;

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);

                var responseTask = client.GetAsync("/api/products/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();
                    product = readTask.Result;
                }
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);

                var putTask = client.PutAsJsonAsync($"/api/products/{product.ProductID}", product);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(product);
        }

        public ActionResult Delete(int id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);

                var responseTask = client.GetAsync("/api/products/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();
                    product = readTask.Result;
                }
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(Product product, int id)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);

                var deleteTask = client.DeleteAsync($"/api/products/{id.ToString()}");
                deleteTask.Wait();
                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }
    }
}