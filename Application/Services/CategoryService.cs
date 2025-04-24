using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;
using MapsterMapper;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(CreateOrEditCategoryDTO createOrEditCategoryDto)
    {
        var category = createOrEditCategoryDto.Adapt<Category>();
        _unitOfWork.CategoryRepo.Add(category);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<CategoryDTO> GetByIdAsync(long Id)
    {
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(Id);
        return category.Adapt<CategoryDTO>();
    }

    public async Task<FilterResponseModel<CategoryDTO>> GetAllAsync(Filter filter)
    {
        var query =  (await _unitOfWork.CategoryRepo.GetAllAsync()).AsQueryable();
        var categories =  query.ApplyFilter(filter);
        var result =  categories.Adapt<List<CategoryDTO>>();
        return new FilterResponseModel<CategoryDTO>()
        {
            Data = result,
            ItemsCount = result.Count(),
        };
    }

    public async Task UpdateAsync(long Id, CreateOrEditCategoryDTO createOrUpdateChannelDto)
    {
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(Id);
        if (category is not null)
        {  
            category.Name = createOrUpdateChannelDto.Name;
            category.Description = createOrUpdateChannelDto.Description;
            _unitOfWork.CategoryRepo.Update(category, category);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(id);
        if (category is not null)
        {
             _unitOfWork.CategoryRepo.Remove(category);
             return await _unitOfWork.CompleteAsync();
        }

        return false;
    }
}