using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;

namespace Application.Services;

public class ModuleService : IModuleService
{
    private readonly IUnitOfWork _unitOfWork;

    public ModuleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FilterResponseModel<ModuleDTO>> GetModuleByCourseId(long courseId)
    {
        var modules = await _unitOfWork.ModuleRepo.GetAllAsync(a => a.CourseId == courseId);
        return new FilterResponseModel<ModuleDTO>()
        {
            Data = modules.Adapt<List<ModuleDTO>>(),
            ItemsCount = modules.Count(),
        };
    }

    public async Task<ModuleDTO> Create(CreateModuleDTO moduleDto)
    {
        var module = moduleDto.Adapt<Module>();
        _unitOfWork.ModuleRepo.Add(module);
        await _unitOfWork.CompleteAsync();
        return module.Adapt<ModuleDTO>();
    }

    public async Task<bool> Delete(long moduleId)
    {
        var module = await _unitOfWork.ModuleRepo.GetByIdAsync(moduleId);
        _unitOfWork.ModuleRepo.Remove(module);
       return await _unitOfWork.CompleteAsync();
    }

    public async Task<ModuleDTO> GetById(long Id)
    {
        try
        {
            var module = await _unitOfWork.ModuleRepo.GetByIdAsync(Id, includeProperties:"Course");
            return module.Adapt<ModuleDTO>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}