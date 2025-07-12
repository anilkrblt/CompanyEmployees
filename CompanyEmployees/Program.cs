using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog; 

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();


// add services to container
builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    // app.UseHsts() will add middleware for using HSTS, which adds the
    // Strict-Transport-Security header.
    app.UseHsts();

// The UseHttpRedirection method is used to add the middleware for the
// redirection from HTTP to HTTPS.
app.UseHttpsRedirection();

// app.UseStaticFiles() enables using static files for the request.
// If  we don’t set a path to the static files directory, it will use a wwwroot
// folder in our project by default.
app.UseStaticFiles();

// app.UseForwardedHeaders() mevcut isteğe proxy başlıklarını iletecektir
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

app.UseCors("CorsPolicy");

// Also, we can see the UseAuthorization
// method that adds the authorization middleware to the specified
// IApplicationBuilder to enable authorization capabilities.
app.UseAuthorization();


/*
app.Use(async (context, next) => 
{ 
Console.WriteLine($"Logic before executing the next delegate in the Use method"); 
await next.Invoke(); 
Console.WriteLine($"Logic after executing the next delegate in the Use method"); 
}); 
app.Run(async context => 
{ 
Console.WriteLine($"Writing the response to the client in the Run method"); 
await context.Response.WriteAsync("Hello from the middleware component."); 
}); 
*/


app.MapControllers();



app.Run();


// http istek sırası
// Exception Handler -> HSTS -> HttpsRedirection -> Static Files -> Routing -> CORS -> Authentication -> Authorization -> custom middleware
// app.MapGet("/", () => "Hello World!"); böyle custom pipeline oluşturabiliriz
// terminal metod
/*
app.Run(async context => 
{ 
await context.Response.WriteAsync("Hello from the middleware component."); 
});
*/



