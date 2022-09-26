using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MyWebAPI.Constants;
using MyWebAPI.Entities;
using MyWebAPI.Security;

var builder = WebApplication.CreateBuilder(args);

var AllowSpecificOrigins = "AllowSpecificOrigins";
var AllowAllOrigins = "AllowAllOrigins";

var config = builder.Configuration;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>

{

    // Begin Basic Authentication

    c.AddSecurityDefinition(SecurityConfig.BasicNameScheme, new OpenApiSecurityScheme

    {

        Description = "Authenticate using Basic Authentication",

        Type = SecuritySchemeType.Http,

        Name = HeaderNames.Authorization,

        In = ParameterLocation.Header,

        Scheme = "basic"

    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement

    {

        {

            new OpenApiSecurityScheme()

            {

                Reference = new OpenApiReference

                {

                    Type = ReferenceType.SecurityScheme,

                    Id = SecurityConfig.BasicNameScheme

                },

                In = ParameterLocation.Header

            },

            Array.Empty<string>()

        }

    });

    // End Basic Authentication

});

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowSpecificOrigins, builder =>
    {
        builder.WithOrigins(
            "https://www.w3schools.com",
            "http://example.com",
            "http://localhost:4200",
            "http://localhost",
            "http://localhost:1152",
            "http://192.168.99.100:1152"
            )
        .AllowAnyHeader()
        .AllowAnyMethod();
        //.WithMethods("GET", "POST", "HEAD");
    });

    options.AddPolicy("C", builder =>
    {
        builder.WithOrigins(
            "https://bbl.com"
            )
        .AllowAnyHeader()
        .WithMethods("GET");
    });

    options.AddPolicy(AllowAllOrigins, builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<DekDueShopContext>(opt =>
    opt.UseSqlServer(config.GetConnectionString("ConnectionSQLServer"))
);

builder.Services.AddAuthentication(SecurityConfig.AuthenticationWithBasicScheme)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(SecurityConfig.AuthenticationWithBasicScheme, null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();