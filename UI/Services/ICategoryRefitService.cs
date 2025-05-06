using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Refit;

namespace UI.Services;

public interface ICategoryRefitService
{
    [Get("/category/getallcategories")]
    public Task<ServiceResponse<FilterResponseModel<CategoryDTO>>> GetAllCategories([Body] Filter filter);  
    [Post("/Category/AddOrEditCategory")] 
    Task AddOrEditCategory(CreateOrEditCategoryDTO categoryDto);
    [Get("/category/getbyid")]
    public Task<ServiceResponse<CategoryDTO>> GetCategoryById(long id); 
    [Delete("/category/delete")]
    Task<ServiceResponse<object>> Delete(long id);
}

