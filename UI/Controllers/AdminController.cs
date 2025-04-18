using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Services;

namespace UI.Controllers;

[Authorize(Roles = nameof(Role.Admin))]
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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRefitService.GetAllCategories();
        var data = categories.Data.Select(a => new CreateOrEditCategoryDTO()
        {
            Id = a.Id,
            Name = a.Name,
        }).ToList();
        return Json(new APIResponse(200,result: data));
    }
    
}