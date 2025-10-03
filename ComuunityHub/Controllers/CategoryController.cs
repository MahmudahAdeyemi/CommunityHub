using ComuunityHub.Interfaces.Services;
using ComuunityHub.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace ComuunityHub.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromBody] AddCategoryRequestModel model)
    {
        var response = await _categoryService.CreateCategory(model);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(string id, [FromBody] UpdateCategoryRequestModel model)
    {
        var response = await _categoryService.UpdateCategory(id, model);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        var response = await _categoryService.DeleteCategory(id);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(string id)
    {
        var response = await _categoryService.GetCategoryById(id);

        if (response.Status)
            return Ok(response);

        return NotFound(response);
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories()
    {
        var response = await _categoryService.GetAllCategories();

        if (response.Status)
            return Ok(response);

        return NotFound(response);
    }
    
    [HttpGet("tree")]
    public async Task<IActionResult> GetCategoryTree()
    {
        var response = await _categoryService.GetCategoryTree();

        if (response.Status)
            return Ok(response);

        return NotFound(response);
    }
}