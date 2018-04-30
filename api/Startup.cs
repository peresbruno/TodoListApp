using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace api
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
            services.AddEntityFrameworkInMemoryDatabase()
                .Configure<JWTSettings>(Configuration.GetSection("JWTSettings"))
                .AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("Users"))
                .AddDbContext<TodoItemContext>(opt => opt.UseInMemoryDatabase("Todos"))
                .AddDbContext<CredentialsContext>(opt => opt.UseInMemoryDatabase("Credentials"))
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<CredentialsContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;

                    return Task.CompletedTask;
                };
            });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            

            app.UseAuthentication();
            
            app.UseMvc();
        }
    }
}