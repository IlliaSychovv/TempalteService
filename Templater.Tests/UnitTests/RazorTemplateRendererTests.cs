using Templater.Application.Services;

namespace TestProject1.UnitTests
{
    public class RazorTemplateRendererTests
    {
        private readonly RazorTemplateRenderer _renderer;

        public RazorTemplateRendererTests()
        {
            _renderer = new RazorTemplateRenderer();
        }

        [Fact]
        public async Task RenderAsync_ShouldRenderTemplate_WhenTemplateIsValid()
        {
            string template = "Hello @Model.Name!";
            var model = new
            {
                Name = "John"
            };

            var result = await _renderer.RenderAsync(template, model);

            Assert.Equal("Hello John!", result);
        }

        [Fact]
        public async Task RenderAsync_ShouldThrowRuntimeBinderException_WhenTemplateIsInvalid()
        {
            string invalidTemplate = "Hello @Model.NonExistentProperty!";
            var model = new
            {
                Name = "John"
            };

            await Assert.ThrowsAsync<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() =>
                _renderer.RenderAsync(invalidTemplate, model));
        }

    }
}