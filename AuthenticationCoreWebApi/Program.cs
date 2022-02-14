
using AuthenticationCoreWebApi.Models;
using AuthenticationCoreWebApi.Services;
using Microsoft.IdentityModel.Tokens;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddSingleton<IBookService,BookService>();
builder.Services.AddSingleton<IUserService,UserService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey=true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});


builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Maps


app.MapGet("/test", () => "Vala, It worked!");
app.MapPost("/create", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")](Book book, IBookService service) => Create(book, service));
app.MapGet("/get", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, standart")](int id, IBookService service) => Get(id, service));
app.MapGet("/getAll", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, standart")](IBookService service) => GetAll(service));
app.MapPut("/update", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")](Book book, IBookService service) => Update(book, service));
app.MapDelete("/delete", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")] (int id, IBookService service) => Delete(id, service));
app.MapPost("/login",  (UserLogin login, IUserService service) => Login(login, service));



#endregion

#region Methods

IResult Create(Book book, IBookService service)
{
    return Results.Ok(service.Create(book));
}

IResult GetAll(IBookService service)
{
    var books = service.GetAll();
    if (books.Count > 0)
    {
        return Results.Ok(books);

    }
    else
    {
        return Results.NoContent();


    }
}

IResult Get(int id, IBookService service)
{
    var book = service.Get(id);
    if (book != null)
    {
        return Results.Ok(book);
    }
    return Results.NotFound();
}

IResult Delete(int id, IBookService service)
{
    var result = service.Delete(id);
    if (result)
    {
        return Results.Ok(true);
    }
    else
    {
        return Results.BadRequest();
    }

}

IResult Update(Book book, IBookService service)
{
    var result = service.Update(book);
    if (result != null)
    {
        return Results.Ok(result);
    }
    else
    {
        return Results.BadRequest();
    }
}

IResult Login(UserLogin login, IUserService service)
{
    if (!string.IsNullOrEmpty(login.UserName)&&!string.IsNullOrEmpty(login.Password))
    {   
        var loggedInUser = service.Get(login);
        if (loggedInUser==null)
        {
            return Results.NotFound();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,loggedInUser.UserName),
            new Claim(ClaimTypes.Email,loggedInUser.EmailAdress),
            new Claim(ClaimTypes.GivenName,loggedInUser.GivenName),
            new Claim(ClaimTypes.Role,loggedInUser.Role),
            new Claim(ClaimTypes.Surname,loggedInUser.SurName)

        };

        var token = new JwtSecurityToken
            (
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            notBefore: DateTime.Now,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)


            );

        var TokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(TokenString);
    }
    else
    {
        return Results.BadRequest();
    }
}

#endregion

app.UseHttpsRedirection();


app.UseSwaggerUI();
app.Run();