
using AuthenticationCoreWebApi.Models;
using AuthenticationCoreWebApi.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IBookService,BookService>();
builder.Services.AddSingleton<IUserService,UserService>();


var app = builder.Build(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/create",(Book book, IBookService service)=>Create(book,service));
app.MapGet("/get", (int id, IBookService service) => Get(id, service));
app.MapGet("/getAll", (IBookService service) => GetAll(service));
app.MapPut("/update", (Book book, IBookService service) => Update(book, service));
app.MapDelete("/delete", (int id, IBookService service) => Delete(id, service));



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


app.UseHttpsRedirection();


app.Run();