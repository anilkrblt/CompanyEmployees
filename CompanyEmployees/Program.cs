using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

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

app.MapControllers();

// app.MapGet("/", () => "Hello World!"); böyle custom pipeline oluşturabiliriz

app.Run();
