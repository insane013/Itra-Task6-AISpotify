using Task6.Services;
using Task6.Services.CoverImageService;
using Task6.Services.SongGenerationService;
using Task6.Services.SongTextInfoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ISongGenerationService, SongGenerationService>();
builder.Services.AddTransient<ISongTextInfoService, SongTextInfoService>();
builder.Services.AddTransient<ICoverImageService, CoverImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();

app.Run();
