using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAPIUsuario.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbUsuarioContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))    
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

//----- FORMA 1 PARA QUE SE ABRA DIRECTAMENTE EL SWAGGER CUANDO EJECUTO LA API (PERO MUESTRA EL NOMBRE DEL PROYECTO COMO ENDPOINT)
//app.MapGet("/", (HttpContext context) =>
//{
//    context.Response.Redirect("/swagger/index.html", permanent: false);
//});

//----- FORMA 2 PARA QUE SE ABRA DIRECTAMENTE EL SWAGGER CUANDO EJECUTO LA API (NO MUESTRA EL NOMBRE DEL PROYECTO COMO ENDPOINT)
app.Use(async (context, next) => {
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html", permanent: false);
        return;
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
