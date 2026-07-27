namespace Identity.ArchitectureTests;

public class LayerTests : BaseArchitectureTest
{
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_Contracts()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn("Contracts")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_OtherBuildingBlocks()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn("BuildingBlocks.Application")
            .And()
            .NotHaveDependencyOn("BuildingBlocks.Infrastructure")
            .And()
            .NotHaveDependencyOn("BuildingBlocks.Presentation")
            .And()
            .NotHaveDependencyOn("BuildingBlocks.IntegrationEvents")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn("IdentityService.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn("IdentityService.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldHaveDependencyOn_BuildingBlocksApplication()
    {
        ApplicationAssembly.GetReferencedAssemblies()
            .Select(a => a.Name)
            .Should()
            .Contain("BuildingBlocks.Application");
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn("IdentityService.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_ApiLayer()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn("IdentityService.API")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void InfrastructureLayer_ShouldHaveDependencyOn_BuildingBlocksInfrastructure()
    {
        InfrastructureAssembly.GetReferencedAssemblies()
            .Select(a => a.Name)
            .Should()
            .Contain("BuildingBlocks.Infrastructure");
    }
}
