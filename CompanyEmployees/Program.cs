var builder = WebApplication.CreateBuilder(args);


// add services to container
builder.Services.AddControllers()


var app = builder.Build();
// configure the http request pipeline


app.MapGet("/", () => "Hello World!");



app.Run();
