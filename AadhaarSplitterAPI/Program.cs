using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AadhaarSplitterAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddScoped<IAadhaarService, AadhaarService>();

var app = builder.Build();

// Configure the application
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Exception handling and security for non-development environment
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Map endpoints
app.MapRazorPages();
app.MapControllers();

app.Run();
