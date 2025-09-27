using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Templater.Application.Interfaces.Repositories;
using Templater.Application.Interfaces.Services;
using Templater.Application.Services;
using Templater.Application.Validators;
using Templater.Infrastructure.Data;
using Templater.Infrastructure.Repositories;
using Templater.Infrastructure.Services;
using Templater.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTemplateValidator>();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

app.UseCors("AllowReactApp");

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();