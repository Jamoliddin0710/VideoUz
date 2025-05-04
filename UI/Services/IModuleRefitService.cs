using Application.DTOs;
using Refit;

namespace UI.Services;

public interface IModuleRefitService
{
    [Post("/module/create")]
    Task<ServiceResponse<ModuleDTO>> Create([Body] CreateModuleDTO dto);
    
    [Get("/module/getmodulesbycourses")]
    Task<ServiceResponse<FilterResponseModel<ModuleDTO>>> GetModulesByCourses([Query] long courseId);

}