using Application.DTOs;

namespace Application.ServiceContract;

public interface IModuleService
{
    Task<FilterResponseModel<ModuleDTO>> GetModuleByCourseId(long courseId);
    Task<ModuleDTO> Create(CreateModuleDTO moduleDto);
}