using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Templater.Application.Interfaces.Repositories;
using Templater.Application.Interfaces.Services;
using Templater.Application.Services;
using Templater.Application.Validators;
using Templater.Infrastructure.Data;
using Templater.Infrastructure.Repositories;
using Templater.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTemplateValidator>();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITemplaterRepository, TemplaterRepository>();
builder.Services.AddScoped<ITemplaterService, TemplaterService>();
builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();
builder.Services.AddSingleton<IRazorTemplateRenderer, RazorTemplateRenderer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature != null)
        {
            var ex = exceptionHandlerFeature.Error;
            await exceptionHandler.TryHandleAsync(context, ex, CancellationToken.None);
        }
    });
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();