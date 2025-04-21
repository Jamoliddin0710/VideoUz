using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refit;
using UI.Services;

namespace UI.Controllers;


public class AdminController : Controller
{
    private readonly ICategoryRefitService _categoryRefitService;

    public AdminController(ICategoryRefitService categoryRefitService)
    {
        _categoryRefitService = categoryRefitService;
    }

    public async Task<IActionResult> Category()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddorEditCategory(CreateOrEditCategoryDTO category)
    {
       
         await _categoryRefitService.AddOrEditCategory(category);
         if (category.Id != 0)
         {
             var oldcategory = await _categoryRefitService.GetCategoryById(category.Id);
             return Json(new APIResponse(200, title: "updated", message: $"category of {oldcategory?.Data?.Name} has renaimed to {category.Name}", result: true));
         }
         return Json(new APIResponse(200, title: $"{category.Name} succesfully created", result: true));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRefitService.GetAllCategories();
        var data = categories?.Data?.Data?.Select(a => new CreateOrEditCategoryDTO()
        {
            Id = a.Id,
            Name = a.Name,
        }).ToList();
        return Json(new APIResponse(200, result: data));
    }
    
}