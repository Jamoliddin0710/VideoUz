using Application.DTOs;
using Application.Models;
namespace Application.ServiceContract;

public interface ICategoryService
{
    // Create a new category
    Task CreateAsync(CreateOrEditCategoryDTO orUpdateChannel);

    // Get a category by its ID
    Task<CategoryDTO> GetByIdAsync(long Id);
    

    // Get all categories
    Task<FilterResponseModel<CategoryDTO>> GetAllAsync();

    // Update an existing category
    Task UpdateAsync(long Id, CreateOrEditCategoryDTO createOrUpdateChannelDto);

    // Delete a category by its ID
    Task<bool> DeleteAsync(long id);
}