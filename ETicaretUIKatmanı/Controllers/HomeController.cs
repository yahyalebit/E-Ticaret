using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ETicaretUIKatmanı.Models;
using ETicaretDalKatmanı.Abstract;
using ETicaretDalKatmanı.Concrete;
using ETicaretDataKatmanı.ViewModels;

namespace ETicaretUIKatmanı.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryDal _categoryDal;
    private readonly IProductDal _productDal;


    public HomeController(ILogger<HomeController> logger, ICategoryDal categoryDal, IProductDal productDal )
    {
        _logger = logger;
        _categoryDal = categoryDal;
        _productDal = productDal;
    }

    public IActionResult Index()
    {
        var products = _productDal.GetAll(p=> p.IsHome && p.IsApproved);
        return View(products);
    }
    public IActionResult List(int? id)
    {
        ViewBag.id = id;
        var products = _productDal.GetAll(x => x.IsApproved);


        if (id!= null)//id değeri varsa
        {
            products = products.Where(p => p.CategoryId == id).ToList(); // Belirtilen kategoriye göre filtrele
        }
        var models = new ListViewModel()
        {
            Categories = _categoryDal.GetAll(),
            Products = products
        };

        return View(models);
    }

    public IActionResult Details(int id)
    {
        var product = _productDal.GetById(id);
        return View(product);
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
}
