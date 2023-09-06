

using Tweetbook.Extensions;
using Tweetbook.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.ConfigureDataContext();
builder.Services.ConfigurePostScoped();
builder.Services.ConfigureIdentity();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var swaggerOptions = new SwaggerOptions();
app.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);


app.UseSwagger(options =>
{
    options.RouteTemplate = swaggerOptions.JsonRoute;
});


app.UseSwaggerUI(options =>
{
    options
        .SwaggerEndpoint(swaggerOptions.UIEndPoint,
                            swaggerOptions.Description);
});

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
