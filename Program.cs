var builder = WebApplication.CreateBuilder(args);

var AllowSpecificOrigins = "AllowSpecificOrigins";
var AllowAllOrigins = "AllowAllOrigins";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

    options.AddPolicy(AllowAllOrigins, builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();