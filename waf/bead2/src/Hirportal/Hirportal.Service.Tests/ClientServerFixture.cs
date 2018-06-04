using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hirportal.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Reflection;

namespace Hirportal.Service.Tests
{
    public class DatabaseSeeder
    {
        private readonly UserManager<Author> _userManager;
        private readonly NewsContext _context;

        public DatabaseSeeder(NewsContext context, UserManager<Author> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            Author testAuthor = new Author() { UserName = "test", Name = "test" };
            var result = _userManager.CreateAsync(testAuthor, "test");

            Article testArticle = new Article()
            {
                Title = "Title",
                Author = testAuthor,
                Content = "Content",
                Description = "Description",
                Leading = false
            };
            _context.Authors.Add(testAuthor);
            _context.Articles.Add(testArticle);

            _context.SaveChanges();
        }
    }

    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NewsContext>(options =>
                options.UseInMemoryDatabase("HirtportalTestDb")
            );

            services.AddIdentity<Author, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<NewsContext>()
            .AddDefaultTokenProviders();

            services.AddTransient<DatabaseSeeder>();

            services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Hirportal.Service")));
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Now seed the database
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();
                await seeder.Seed();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }

    public class ClientServerFixture : IDisposable
    {
        public TestServer TestServer { get; private set; }
        public HttpClient Client { get; private set; }

        public ClientServerFixture()
        {
            var integrationTestsPath = PlatformServices.Default.Application.ApplicationBasePath;
            var applicationPath = Path.GetFullPath(Path.Combine(integrationTestsPath, "../../../../Hirportal.Service"));

            TestServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseEnvironment("Development"));
            Client = TestServer.CreateClient();
        }

        public void Dispose()
        {
            TestServer?.Dispose();
            Client?.Dispose();
        }
    }
}
