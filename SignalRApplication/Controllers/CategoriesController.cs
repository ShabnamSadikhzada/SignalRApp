using Microsoft.AspNetCore.Mvc;
using SignalRApplication.Repositories;

namespace SignalRApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    [HttpGet]
    public IActionResult Get()
    {
        var types = _categoryRepository.GetAll();
        return Ok(types);
    }
}