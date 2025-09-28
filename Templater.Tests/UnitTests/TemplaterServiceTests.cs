using Moq;
using Templater.Application.DTO;
using Templater.Application.Interfaces.Repositories;
using Templater.Application.Interfaces.Services;
using Templater.Application.Services;
using Templater.Domain.Entities;

namespace TestProject1.UnitTests;

public class TemplaterServiceTests
{
    private readonly Mock<ITemplaterRepository> _mockRepository;
    private readonly Mock<IAngleSharpHtmlSanitizer> _mockAngleSanitizer;
    private readonly TemplaterService _templaterService;

    public TemplaterServiceTests()
    {
        _mockRepository = new Mock<ITemplaterRepository>();
        _mockAngleSanitizer = new Mock<IAngleSharpHtmlSanitizer>();

        _templaterService = new TemplaterService(
            _mockRepository.Object
        );
    }

    [Fact]
    public async Task GetTemplatesAsync_ShouldReturnPagedResponse_WhenTemplatesExist()
    {
        var templates = new List<Template>
        {
            new Template
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            }
        };
        _mockRepository.Setup(r => r.GetAllTemplatesAsync(1, 10)).ReturnsAsync(templates);
        _mockRepository.Setup(r => r.CountTemplatesAsync()).ReturnsAsync(1);

        var result = await _templaterService.GetTemplatesAsync();

        Assert.NotNull(result);
        Assert.Single(result.Item);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task GetTemplatesAsync_ShouldReturnEmpty_WhenNoTemplatesExist()
    {
        _mockRepository.Setup(r => r.GetAllTemplatesAsync(1, 10)).ReturnsAsync(new List<Template>());
        _mockRepository.Setup(r => r.CountTemplatesAsync()).ReturnsAsync(0);

        var result = await _templaterService.GetTemplatesAsync();

        Assert.NotNull(result);
        Assert.Empty(result.Item);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task GetTemplateById_ShouldReturnTemplate_WhenTemplateExists()
    {
        var id = Guid.NewGuid();
        var template = new Template
        {
            Id = id,
            Name = "Test"
        };
        _mockRepository.Setup(r => r.GetTemplateByIdAsync(id)).ReturnsAsync(template);

        var result = await _templaterService.GetTemplateById(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetTemplateById_ShouldReturnNull_WhenTemplateDoesNotExist()
    {
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetTemplateByIdAsync(id)).ReturnsAsync((Template)null);

        var result = await _templaterService.GetTemplateById(id);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddTemplateAsync_ShouldAddTemplate_WhenDtoIsValid()
    {
        var dto = new CreateTemplateDto
        {
            Name = "Test",
            HtmlContent = "<b>Test</b>"
        };
        _mockAngleSanitizer.Setup(s => s.Sanitize(dto.HtmlContent)).Returns(dto.HtmlContent);

        var result = await _templaterService.AddTemplateAsync(dto);

        _mockRepository.Verify(r => r.AddTemplateAsync(It.IsAny<Template>()), Times.Once);
        Assert.Equal(dto.Name, result.Name);
    }

    [Fact]
    public async Task AddTemplateAsync_ShouldThrowException_WhenRepositoryFails()
    {
        var dto = new CreateTemplateDto
        {
            Name = "Test",
            HtmlContent = "<b>Test</b>"
        };
        _mockAngleSanitizer.Setup(s => s.Sanitize(dto.HtmlContent)).Returns(dto.HtmlContent);
        _mockRepository.Setup(r => r.AddTemplateAsync(It.IsAny<Template>())).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _templaterService.AddTemplateAsync(dto));
    }

    [Fact]
    public async Task UpdateTemplateAsync_ShouldUpdateTemplate_WhenTemplateExists()
    {
        var id = Guid.NewGuid();
        var template = new Template
        {
            Id = id,
            Name = "Old",
            HtmlContent = "old"
        };
        var dto = new UpdateTemplateDto
        {
            Name = "New",
            HtmlContent = "<b>New</b>"
        };

        _mockRepository.Setup(r => r.GetTemplateByIdAsync(id)).ReturnsAsync(template);
        _mockAngleSanitizer.Setup(s => s.Sanitize(dto.HtmlContent)).Returns(dto.HtmlContent);

        var result = await _templaterService.UpdateTemplateAsync(id, dto);

        _mockRepository.Verify(r => r.UpdateTemplateAsync(template), Times.Once);
        Assert.True(result);
        Assert.Equal("New", template.Name);
    }

    [Fact]
    public async Task UpdateTemplateAsync_ShouldThrowException_WhenTemplateDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateTemplateDto
        {
            Name = "New", HtmlContent = "<b>New</b>"
        };
        _mockRepository.Setup(r => r.GetTemplateByIdAsync(id)).ReturnsAsync((Template)null);

        await Assert.ThrowsAsync<NullReferenceException>(() => _templaterService.UpdateTemplateAsync(id, dto));
    }

    [Fact]
    public async Task DeleteTemplateAsync_ShouldDeleteTemplate_WhenTemplateExists()
    {
        var id = Guid.NewGuid();

        var result = await _templaterService.DeleteTemplateAsync(id);

        _mockRepository.Verify(r => r.DeleteTemplateAsync(id), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteTemplateAsync_ShouldThrowException_WhenRepositoryFails()
    {
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteTemplateAsync(id)).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _templaterService.DeleteTemplateAsync(id));
    }
}