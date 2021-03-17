using Insurance.Api.DependencyInjection;
using Insurance.Api.InputFormatter;
using Insurance.Data;
using Insurance.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Utilities.Polly.DependencyInjection;

namespace Insurance.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InsuranceContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("EntityContext"));
            });

            services.AddHealthChecks();
            //Adding custom input formatter to allow uploading CSV files with surcharge data.
            services.AddControllers(options =>
            {
                var csvFormatterOptions = new CsvFormatterOptions { CsvDelimiter = "," };
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
                options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions));
            });
            services.AddConfigurationServices(configuration: Configuration);
            services.RegisterPollyPoliciesServices();
            services.AddHttpClientServices();
            services.AddProductServices();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // Adding middleware to catch exception instead of the redundant "try catch" in Controllers.
                ExceptionHandling(applicationBuilder: app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //Support health check
                endpoints.MapHealthChecks("/health");
            });
        }

        private void ExceptionHandling(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                });
            });
        }
    }
}