using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Services;

namespace UI.Controllers;

public class AdminController(ICategoryRefitService _categoryRefitService, IAccountRefitClient accountRefitClient)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Category()
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(CategoryDTO.Name), "Name" },
            { nameof(CategoryDTO.Description), "Description" },
        };
        return View("Category1");
    }

    [HttpPost]
    public async Task<IActionResult> AddorEditCategory([FromBody] CreateOrEditCategoryDTO category)
    {
        if (category.Id != 0)
        {
            var oldcategory = await _categoryRefitService.GetCategoryById(category.Id);
            await _categoryRefitService.AddOrEditCategory(category);
            TempData["notification"] =
                $"true; updated; category of {oldcategory?.Data?.Name} has renaimed to {category.Name}; modal";
            return Json(new APIResponse(200, title: "updated",
                message: $"category of {oldcategory?.Data?.Name} has renaimed to {category.Name}", result: true));
        }

        await _categoryRefitService.AddOrEditCategory(category);
        TempData["notification"] = $"true; created; {category.Name} succesfully created; modal";
        return Json(new APIResponse(200, title: $"{category.Name} succesfully created", result: true));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategories([FromBody] Filter filter)
    {
        var categories = await _categoryRefitService.GetAllCategories(filter);
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

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> Users()
    {
        ViewBag.SearchFields = typeof(UserDTO)
            .GetProperties()
            .Where(p => p.Name != nameof(UserDTO.Id))
            .ToDictionary(p => p.Name, p => p.Name);

        ViewBag.Roles = (await accountRefitClient.GetAllRoles()).Data.Data;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers(Filter filter)
    {
        var users = await accountRefitClient.GetAllUsers();
        return Json(new APIResponse(200, result: users.Data.Data));
    }

    [HttpPost]
    public async Task<IActionResult> SaveUser([FromBody] UpdateUserDTO userDto)
    {
        var result = await accountRefitClient.Update(userDto);
        if (result.Data)
        {
            return Json(new APIResponse(200));
        }

        return Json(new APIResponse(400));
    }

    public async Task<IActionResult> DeleteUser(long id)
    {
        var result = await accountRefitClient.Delete(id);
        if (result.Data)
        {
            return Json(new APIResponse(200));
        }

        return Json(new APIResponse(400));
    }
}