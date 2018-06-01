using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hirportal.Models;
using Hirportal.Persistence;

namespace Hirportal
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
            services.AddDbContext<NewsContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<Author, IdentityRole<int>>()
                .AddEntityFrameworkStores<NewsContext>()
                .AddDefaultTokenProviders();

            services.Configure<ImagePathConfig>(Configuration);//_ => new ImagePathConfig() { ImagePath = Configuration.GetValue<string>("ImagePath") }

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<NewsContext>();
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(
                app,
                System.IO.Path.Combine(env.ContentRootPath, Configuration.GetValue<string>("DummyImages")),
                System.IO.Path.Combine(env.WebRootPath, Configuration.GetValue<string>("ImagePath"))
            );
        }
    }
}
