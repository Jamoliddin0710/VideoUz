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
        
         if (category.Id != 0)
         {
             var oldcategory = await _categoryRefitService.GetCategoryById(category.Id);
             await _categoryRefitService.AddOrEditCategory(category);
             TempData["notification"] = $"true; updated; category of {oldcategory?.Data?.Name} has renaimed to {category.Name}; modal";
             return Json(new APIResponse(200, title: "updated", message: $"category of {oldcategory?.Data?.Name} has renaimed to {category.Name}", result: true));
         }
         
         await _categoryRefitService.AddOrEditCategory(category);
         TempData["notification"] = $"true; created; {category.Name} succesfully created; modal";
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
            Description = a.Description,
        }).ToList();
        return Json(new APIResponse(200, result: data));
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _categoryRefitService.Delete(id);
        var result = Convert.ToBoolean(response.Data);
        if (result)
        {
            return Json(new APIResponse(200));
        }
      
        return Json(new APIResponse(400));
    }
    
}