using AutoSelect.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Додаємо сервіси
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration["ConnectionStrings:DefaultConnection"],
        new MySqlServerVersion(new Version(8, 0, 26))));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoSelect API", Version = "v1" });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});



var app = builder.Build();
app.UseCors("AllowAll");
// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoSelect API v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Вимкнути HTTPS для розробки
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages(); // Для Razor Pages (/Cars, /Clients, тощо)
app.MapControllers(); // Для API (/api/cars, /api/clients, тощо)
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");
    return System.Threading.Tasks.Task.CompletedTask;
});

app.Run();