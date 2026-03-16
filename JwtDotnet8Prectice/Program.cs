

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https    ://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT Token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var key = builder.Configuration["JwtSection:SecretKey"];
var issurer = builder.Configuration["JwtSection:Issuer"];
var Audience = builder.Configuration["JwtSection:autdience"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
 )    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
       ValidateIssuer = true,
       ValidateAudience = true,
       RequireExpirationTime = true,
       RoleClaimType = ClaimTypes.Role,
       ValidAudience =  Audience,
       ValidIssuer = issurer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( key))
    });


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 50;
        opt.Window = TimeSpan.FromMinutes(11);
    });
});
builder.Services.AddRateLimiter(options => options.AddFixedWindowLimiter("HitCoundPolicy",
    opt => {
        opt.PermitLimit = 77;
        opt.Window = TimeSpan.FromMicroseconds(2);
        }
));

builder.Services.AddRateLimiter(options => options.AddFixedWindowLimiter("LimitTEst", opt =>
{
    opt.PermitLimit = 1;
    opt.Window = TimeSpan.FromMicroseconds(5);
}));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
///////////////////////////////
/////
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Security.Claims;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var key = builder.Configuration["JwtSection:SecretKey"];

//var issuer = builder.Configuration["JwtSection:Issuer"];
//var audience = builder.Configuration["JwtSection:autdience"];

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        RequireExpirationTime = true,
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(key)
//        ),

//        ValidIssuer = issuer,
//        ValidAudience = audience,
//        RoleClaimType = ClaimTypes.Role
//    };
//});

//builder.Services.AddAuthorization();

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();