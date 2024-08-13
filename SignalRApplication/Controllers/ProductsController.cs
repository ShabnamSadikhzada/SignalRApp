using Microsoft.AspNetCore.Mvc;
using SignalRApplication.Repositories;

namespace SignalRApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    
    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }



    [HttpGet]
    public IActionResult Get()
    {
        var types = _productRepository.GetAll();
        return Ok(types);
    }
}
