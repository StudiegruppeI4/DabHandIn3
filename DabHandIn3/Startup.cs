using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DabHandIn3.Models;
using DabHandIn3.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DabHandIn3
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
            services.Configure<DabHandIn3DatabaseSettings>(Configuration.GetSection(nameof(DabHandIn3DatabaseSettings)));

            services.AddSingleton<IDabHandIn3DatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<DabHandIn3DatabaseSettings>>().Value);

            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
            services.AddSingleton<UserService>();
            services.AddSingleton<PostService>();
            services.AddSingleton<CircleService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Seeding Data
            var settings =
                (IDabHandIn3DatabaseSettings) app.ApplicationServices.GetService(typeof(IDabHandIn3DatabaseSettings));
            SeedData(settings);
        }

        private void SeedData(IDabHandIn3DatabaseSettings settings)
        {
            UserService userService = new UserService(settings);
            CircleService circleService = new CircleService(settings);
            PostService postService = new PostService(settings);

            // Seeding Users
            List<User> users = userService.Get();

            if (users != null)
            {
                foreach (var user in users)
                    userService.Remove(user);
            }

            List<User> usersToInsert = new List<User>()
            {
                new User()
                {
                    UserName = "Ros",
                    Age = 21,
                    Email = "Ros@Ros.ros",
                    Gender = "Male"
                },
                new User()
                {
                    UserName = "Roman",
                    Age = 21,
                    Email = "Roman@Roman.roman",
                    Gender = "Male"
                },
                new User()
                {
                    UserName = "Bommy",
                    Age = 22,
                    Email = "Bom@Bom.bommy",
                    Gender = "Male"
                },
                new User()
                {
                    UserName = "Agri",
                    Age = 22,
                    Email = "Agri@Agri.agri",
                    Gender = "Male"
                }
            };

            foreach (var user in usersToInsert)
                userService.Create(user);

            // Seeding Circles
            List<Circle> circles = circleService.Get();

            if (circles != null)
            {
                foreach (var circle in circles)
                    circleService.Remove(circle);
            }

            List<Circle> circlesToInsert = new List<Circle>()
            {
                new Circle()
                {
                    Admin = usersToInsert[0],
                    CircleName = "Rosendals Circle",
                    Posts = new List<Post>(),
                    Users = new List<User>()
                    {
                        usersToInsert[0],
                        usersToInsert[1]
                    }
                },
                new Circle()
                {
                    Admin = usersToInsert[2],
                    CircleName = "Rosendals Circle",
                    Posts = new List<Post>(),
                    Users = new List<User>()
                    {
                        usersToInsert[2],
                        usersToInsert[3]
                    }
                }
            };

            foreach (var circle in circlesToInsert)
                circleService.Create(circle);
        }
    }
}
