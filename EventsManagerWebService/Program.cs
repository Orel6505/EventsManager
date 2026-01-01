using EventsManager.Data_Access_Layer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "EventsManager API",
		Version = "v1",
		Description = "API for managing events, halls, menus, and orders"
	});
});

// Register database context and unit of work with configuration
builder.Services.AddScoped<DbContext>(provider =>
{
	var configuration = provider.GetRequiredService<IConfiguration>();
	return new OleDbContext(configuration);
});
builder.Services.AddScoped<LibraryUnitOfWork>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add CORS policy
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowWebApp", policy =>
	{
		policy.WithOrigins(
				builder.Configuration["AllowedOrigins"] ?? "https://localhost:7001")
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Events Manager API v1");
	});
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseFileServer();

// Enable CORS
app.UseCors("AllowWebApp");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapHealthChecks("/health");

// Set custom static HTML page as the homepage
app.MapGet("/", () => Results.Redirect("/Index.html"));

app.Run();
