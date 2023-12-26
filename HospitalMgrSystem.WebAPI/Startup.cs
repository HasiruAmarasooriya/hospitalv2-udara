using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalMgrSystem.Service;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystem.Service.Room;
using HospitalMgrSystem.Service.Specialist;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Admission;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.Investigation;
using HospitalMgrSystem.Service.Item;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.Channeling;

namespace HospitalMgrSystem.WebAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HospitalMgrSystem.WebAPI", Version = "v1" });
            });


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ISpecialistsService, SpecialistsService>();
            services.AddScoped<IConsultantService, ConsultantService>();
            services.AddScoped<IAdmissionService, AdmissionService>();
            services.AddScoped<IDrugsService, DrugsService>();
            services.AddScoped<IInvestigationService, InvestigationService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IChannelingSchedule, ChannelingScheduleService>();
            services.AddScoped<IChannelingService, ChannelingService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HospitalMgrSystem.WebAPI v1"));
            }

            app.UseCors();

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
