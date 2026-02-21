using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Controllers
builder.Services.AddControllers();

// 🔹 JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(
        "THIS_IS_A_SUPER_SECRET_KEY_FOR_HS256_AUTH_SERVICE_12345"
    )
)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// 🔹 IMPORTANT ORDER 🔹
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowReact");

app.UseRouting();            // ✅ MISSING EARLIER
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();