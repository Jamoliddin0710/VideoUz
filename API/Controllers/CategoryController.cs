using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CategoryController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<object>>> AddOrEditCategory([FromBody]CreateOrEditCategoryDTO category)
    {
        if(category.Id != 0)
        {
            await _categoryService.UpdateAsync(category.Id, category);
        }
        else
        {
            await _categoryService.CreateAsync(category);
        }

        return NoContent();
    }
    /*[Authorize(Roles = nameof(Role.Admin))]*/
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<FilterResponseModel<CategoryDTO>>>> GetAllCategories()
    {
       var categories = await _categoryService.GetAllAsync();
       return new ServiceResponse<FilterResponseModel<CategoryDTO>>
       {
           IsSuccessful = true,
           Data = categories,
           Error = null,
       };
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<CategoryDTO>>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return ServiceResponse<CategoryDTO>.Success(category);
    }
}