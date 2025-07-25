using System.Text;
using cookware_react_backend.Context;
using cookware_react_backend.Models;
using cookware_react_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProductServices>();
builder.Services.AddScoped<AdminServices>();
builder.Services.AddScoped<ReviewServices>();
builder.Services.AddScoped<DetailsServices>();
builder.Services.AddSingleton<BlobServices>();
builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
    policy =>
    {
        policy.AllowAnyOrigin().
            AllowAnyHeader().
            AllowAnyMethod();
    });
});

var secretKey = builder.Configuration["JWT:Key"];
var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


// our secret key should match the secret key that we use to issue the token
builder.Services.AddAuthentication(options =>
{
    // this line of code will set the authentification behaviour of our JWt Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // sets the default behaviour for when our auth fails
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //check if the tokens issuer is valid
        ValidateAudience = true, //check if the token's audience is valid  
        ValidateLifetime = true, //ensures that our tokens haven't expired
        ValidateIssuerSigningKey = true, //checking the tokens signature is valid

        ValidIssuer = "https://cookwareinterface-drgnfkhdevbvd6gw.westus-01.azurewebsites.net/",
        ValidAudience = "https://cookwareinterface-drgnfkhdevbvd6gw.westus-01.azurewebsites.net/",
        IssuerSigningKey = signingCredentials
    };
});
//now this is set up, you can make a call to an endpoint that is protected by [Authoruze] by adding a "Authorization" header as the Key with a value of "Bearer (your token here)"

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
