using Microsoft.AspNetCore.Http.Features;
using MIDCloud.API.Extensions;
using MIDCloud.API.Helpers;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddDatabase(configuration.GetSection("Database"));

        builder.Services.AddServices();

        builder.Services.AddCors(options =>
            options.AddPolicy("AllowSpecificOrigins", builder => builder.WithOrigins("http://192.168.0.101:3000")
                                                                        .WithOrigins("http://localhost:3000")
                                                                        .AllowCredentials()
                                                                        .AllowAnyHeader()
                                                                        .AllowAnyMethod()
                                                                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                                                                        .WithExposedHeaders("Authorization")
                                                                        .WithExposedHeaders("max-page")));

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1073741824;
        });

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddDebug();
            loggingBuilder.AddConsole();
        });

        var app = builder.Build();

        app.UseCors("AllowSpecificOrigins");

        app.UseRouting();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseAuthentication();
        //app.UseAuthorization();

        app.UseMiddleware<JwtMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.Run();
    }
}