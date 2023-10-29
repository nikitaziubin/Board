using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder => builder
		.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod());
});


builder.Services.AddSignalR();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();	

app.UseRouting();
app.UseAuthorization();

app.UseCors(); 


app.UseDefaultFiles(new DefaultFilesOptions
{
	DefaultFileNames = new List<string> { "Board.html" }
});
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
	endpoints.MapHub<DrawHub>("/drawHub");
});

app.Run();
