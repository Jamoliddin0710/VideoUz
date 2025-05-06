using Application.DTOs;

namespace Application.ServiceContract;

public interface IModuleService
{
    Task<FilterResponseModel<ModuleDTO>> GetModuleByCourseId(long courseId);
    Task<ModuleDTO> Create(CreateModuleDTO moduleDto);
    Task<bool> Delete(long moduleId);
    Task<ModuleDTO> GetById(long Id);
}