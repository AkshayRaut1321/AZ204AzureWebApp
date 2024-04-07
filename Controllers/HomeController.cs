using AZ204AzureWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace AZ204AzureWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        //private readonly IFeatureManager _featureManager;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHostEnvironment hostEnvironment
            //, IFeatureManager featureManager
            )
        {
            _configuration = configuration;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            //_featureManager = featureManager;
        }

        public async Task<IActionResult> Index()
        {
            string? connString;
            if (_hostEnvironment.IsDevelopment())
            {
                //For retrieving connection string from in-app appsettings.json file
                connString = _configuration.GetConnectionString("SqlConMain");
                //connString = _configuration.GetConnectionString("SqlConStaging");
            }
            else
            {
                //If you want to retrieve connection string from Environment variables section of a web app.
                //connString = _configuration.GetConnectionString("SqlCon");

                //If you want to retrieve from Azure App Configuration resource.
                //connString = _configuration["SqlCon"];

                //Testing for deploying this on Docker on Linux VM on Azure.
                connString = _configuration.GetConnectionString("SqlConMain");
            }
            string query = "SELECT Id, Name, Price FROM dbo.Products";
            DataTable dataTable = new DataTable();
            var products = new List<Product>();

            try
            {
                // Create an instance of SqlDataAdapter to execute the query and fill the DataTable
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connString))
                {
                    // Open the connection, execute the query and fill the DataTable,
                    // then close the connection automatically when done.
                    adapter.Fill(dataTable);
                }

                // Now the DataTable is filled with data from the Products table.
                // You can access the data like this:
                foreach (DataRow row in dataTable.Rows)
                {
                    var product = new Product();
                    product.Id = Convert.ToInt32(row["Id"]);
                    product.Name = row["Name"]!.ToString();
                    product.Price = Convert.ToInt32(row["Price"]);
                    products.Add(product);
                }
                //ViewData["IsBeta"] = await _featureManager.IsEnabledAsync("IsBeta");
            }
            catch (Exception ex)
            {
                // Handle any errors that might have occurred
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        //If this feature flag is not present or false then server will return 404 if this API method is called.
        [FeatureGate("IsBeta")]
        public IActionResult ExecuteRazorFunction()
        {
            var parallelOps = new ParallelOptions();
            parallelOps.MaxDegreeOfParallelism = 10;
            Parallel.For(0, 10000, (a) =>
            {
                Console.Write("test");
            });
            // Your function logic here
            return RedirectToAction("Index");
        }
    }
}