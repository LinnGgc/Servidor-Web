using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.PropertyNamingPolicy = null);

    builder.Services.AddCors();

var app = builder.Build();

app.UseCors(Policy => Policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapGet("/", () => "Hello World!");

app.MapGet("/control-escolar/alumnos",AlumnosRequestHandler.ListarAlumnos);

app.MapPost("/Usuario/Ingresar",UsuariosRequestHandler.Ingresar);

app.MapPost("/Usuario/Contrasena",UsuariosRequestHandler.Recuperar);

app.MapPost("/Usuario/Registro",UsuariosRequestHandler.Registro);

app.MapPost("/Categorias/Crear", CategoriaRequestHanlder.Crear);

app.MapGet("/Categorias/Listar", CategoriaRequestHanlder.Listar);

app.MapGet("/Lenguaje/{idCategoria}",LenguajeRequestHandler.ListarRegistros);

app.MapPost("/Lenguaje/Crear",LenguajeRequestHandler.CrearRegistro);

app.MapDelete("/Lenguaje/{id}",LenguajeRequestHandler.Eliminar);  

app.MapGet("/Lenguaje/Buscar",LenguajeRequestHandler.Buscar);

app.Run();
