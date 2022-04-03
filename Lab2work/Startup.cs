using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab2work
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.Map("/ListOfPeople", ListOfPeople);
            app.Map("/SearchByEmail", SearchByEmail);
            app.Map("/FindSex", FindSex);
            app.Map("/DelPerson", DelPerson);
            app.Map("/AddPers", AddPers);
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Laboratory work 2");
            });

        }
        private static void ListOfPeople(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                People people = new People("D:\\4year\\Diplom\\person.txt");                
                if (people != null)
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(people.DataPeople,options));
                }
                else
                {
                    await context.Response.WriteAsync("Missing data");
                }                              
            });
        }
        private static void SearchByEmail(IApplicationBuilder app)
        {
            People people = new People("D:\\4year\\Diplom\\person.txt");
            app.Run(async context =>
            {
                string email = context.Request.Query["email"];
                await context.Response.WriteAsync(JsonSerializer.Serialize(people.SearchByEmail(email)));
            });
        }
        private static void FindSex(IApplicationBuilder app)
        {
            People people = new People("D:\\4year\\Diplom\\person.txt");
            app.Run(async context =>
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string sex = context.Request.Query["sex"];
                await context.Response.WriteAsync(JsonSerializer.Serialize(people.FilterBySex(sex), options));
            });
        }
        private static void DelPerson(IApplicationBuilder app)
        {
            string filename = "D:\\4year\\Diplom\\person.txt";
            People people = new People(filename);
            app.Run(async context =>
            {
                string email = context.Request.Query["email"];
                string delpersname=people.SearchByEmail(email).FirstName;
                people.DeletePerson(email, filename);
                await context.Response.WriteAsync(string.Format("Видалено {0} зі списку",delpersname));
            });
        }
        private static void AddPers(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                People people = new People("D:\\4year\\Diplom\\person.txt");
                var request = context.Request.Body; 
                using (StreamReader reader = new StreamReader(request))
                {
                    string body = await reader.ReadToEndAsync();
                    Person person = new Person();
                    var form = JsonSerializer.Deserialize<Person>(body);
                    string answer= people.AddPerson(form, "D:\\4year\\Diplom\\person.txt");                    
                    await context.Response.WriteAsync(answer);
                }                
            });
        }
    }
}
