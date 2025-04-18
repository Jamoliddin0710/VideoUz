using Application.DTOs;
using Refit;

namespace UI.Services;

public interface ICategoryRefitService
{
    [Get("/category/getallcategories")]
    public Task<ServiceResponse<List<CategoryDTO>>> GetAllCategories();
}