using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Authorize(Roles = nameof(Role.Admin))]
public class AdminController : BaseApiController
{
   
}