using FluentAssertions;

using Xunit;

namespace KubeOps.Templates.Test.Templates;

[Collection("Template Tests")]
public class EmptyFSharpTest : IDisposable
{
    private readonly TemplateExecutor _executor = new();

    public EmptyFSharpTest(TemplateInstaller _)
    {
    }

    [Fact]
    public async Task Should_Create_Correct_Files()
    {
        await _executor.ExecuteFSharpTemplate("operator-empty");
        _executor.FileExists("Template.fsproj").Should().BeTrue();
        _executor.FileExists("Startup.fs").Should().BeTrue();
        _executor.FileExists("Program.fs").Should().BeTrue();
        _executor.FileExists("appsettings.Development.json").Should().BeTrue();
        _executor.FileExists("appsettings.json").Should().BeTrue();
    }

    [Fact]
    public async Task Should_Add_KubeOps_Reference()
    {
        await _executor.ExecuteFSharpTemplate("operator-empty");
        _executor.FileContains("""
                               PackageReference Include="KubeOps"
                               """, "Template.fsproj").Should().BeTrue();
    }

    [Fact]
    public async Task Should_Add_KubeOps_Reference_Into_Startup_Files()
    {
        await _executor.ExecuteFSharpTemplate("operator-empty");
        _executor.FileContains("services.AddKubernetesOperator() |> ignore", "Startup.fs").Should().BeTrue();
        _executor.FileContains("app.UseKubernetesOperator()", "Startup.fs").Should().BeTrue();
    }

    [Fact]
    public async Task Should_Create_Correct_Program_Code()
    {
        await _executor.ExecuteFSharpTemplate("operator-empty");
        _executor.FileContains(".RunOperatorAsync args", "Program.fs")
            .Should()
            .BeTrue();
    }

    public void Dispose()
    {
        _executor.Dispose();
    }
}