using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DabHandIn3.Models;
using DabHandIn3.Models.Objects;
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
                    Gender = "Male",
                    BlockedUsers = new List<string>(),
                    Following = new List<string>()
                },
                new User()
                {
                    UserName = "Roman",
                    Age = 21,
                    Email = "Roman@Roman.roman",
                    Gender = "Male",
                    BlockedUsers = new List<string>(),
                    Following = new List<string>()
                },
                new User()
                {
                    UserName = "Bommy",
                    Age = 22,
                    Email = "Bom@Bom.bommy",
                    Gender = "Male",
                    BlockedUsers = new List<string>(),
                    Following = new List<string>()
                },
                new User()
                {
                    UserName = "Agri",
                    Age = 22,
                    Email = "Agri@Agri.agri",
                    Gender = "Male",
                    BlockedUsers = new List<string>(),
                    Following = new List<string>()
                }
            };

            foreach (var user in usersToInsert)
                userService.Create(user);

            // Roman follows Rosendal
            usersToInsert[1].Following = new List<string>()
            {
                usersToInsert[0].Id
            };
            userService.Update(usersToInsert[1].Id, usersToInsert[1]);

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
                    Public = true,
                    CircleName = "Rosendals Circle",
                    PostIds = new List<string>(),
                    UserIds = new List<string>()
                    {
                        usersToInsert[0].Id,
                        usersToInsert[1].Id
                    }
                },
                new Circle()
                {
                    Admin = usersToInsert[2],
                    Public = false,
                    CircleName = "Bommys Circle",
                    PostIds = new List<string>(),
                    UserIds = new List<string>()
                    {
                        usersToInsert[2].Id,
                        usersToInsert[3].Id
                    }
                }
            };

            foreach (var circle in circlesToInsert)
                circleService.Create(circle);

            // Seeding Posts
            List<Post> posts = postService.Get();

            if (posts != null)
            {
                foreach (var post in posts)
                    postService.Remove(post);
            }

            List<Post> postsToInsert = new List<Post>()
            {
                new Post()
                {
                    Author = usersToInsert[0],
                    Content = new Content()
                    {
                        Text = "Now this is content boys",
                        Image = null
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Author = usersToInsert[1],
                            Content = "This is not content boys",
                            CreationTime = DateTime.Now
                        }
                    },
                    CreationTime = DateTime.Now.AddMinutes(-1)
                },
                new Post()
                {
                    Author = usersToInsert[0],
                    Content = new Content()
                    {
                        Text = "So bored today, lol",
                        Image = null
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Author = usersToInsert[2],
                            Content = "Roflcopter",
                            CreationTime = DateTime.Now
                        }
                    },
                    CreationTime = DateTime.Now.AddMinutes(-2)
                }
            };

            foreach (var post in postsToInsert)
            {
                postService.Create(post, circlesToInsert[0].Id);
            }
        }
    }
}
