using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
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

    [Authorize(Roles = nameof(Role.Admin))]
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
}