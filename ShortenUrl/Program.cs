using Auth0.AspNetCore.Authentication;

using Redis.OM;

using ShortenUrl.HostedServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new RedisConnectionProvider(builder.Configuration["RedisConnectionString"]));
builder.Services.AddHostedService<IndexCreationService>();
builder.Services
        .AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = builder.Configuration["Auth0:Domain"];
            options.ClientId = builder.Configuration["Auth0:ClientId"];
        });
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


app.Run();

public partial class Program { }