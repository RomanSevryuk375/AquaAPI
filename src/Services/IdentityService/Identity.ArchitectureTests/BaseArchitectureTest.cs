using System.Reflection;
using IdentityService.API.Controllers;
using IdentityService.Application.DTOs;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;

namespace Identity.ArchitectureTests;

public abstract class BaseArchitectureTest
{
    protected static readonly Assembly DomainAssembly = typeof(User).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(LoginResponseDto).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(IdentityDbContext).Assembly;
    protected static readonly Assembly ApiAssembly = typeof(AuthController).Assembly;
}
