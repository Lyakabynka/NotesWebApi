using Notes.Persistence;
using Notes.Application.Common.Mappings;
using System.Reflection;
using Notes.Application.Interfaces;
using Notes.Application;
using Notes.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

#region //Services
//For mapping one object into another
builder.Services.AddAutoMapper(config=>
{
    //We need to take an assembly to pass it to profile from current project.
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));

    //We need to take an assembly to pass it to profile from Application Layer. ( thats why we take INotesDbContext )
    config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

builder.Services.AddCors(options=>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = 
        JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = 
        JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options=>
    {
        options.Authority = "http://localhost:47755";
        options.Audience = "NotesWebAPI";
        options.RequireHttpsMetadata = false;
    });
#endregion
var app = builder.Build();

//for getting DbContext ( cuz its Scoped Service )
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>();
        DbInitializer.Initialize(context);
    }
    catch(Exception ex)
    {
        //todos
    }
}

app.UseCustomExceptionHandler();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
