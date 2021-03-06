using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backEnd
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
    .AddJwtBearer(options =>    
    {    
        options.TokenValidationParameters = new TokenValidationParameters    
        {    
            ValidateIssuer = true,    
            ValidateAudience = true,    
            ValidateLifetime = true,    
            ValidateIssuerSigningKey = true,    
            ValidIssuer = Configuration["Jwt:Issuer"],    
            ValidAudience = Configuration["Jwt:Issuer"],    
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))    
        };    
    });    

            services.Configure<FormOptions>(c => {
                c.MultipartBodyLengthLimit = 400000000;
                c.MultipartHeadersLengthLimit = 450000000;
            });
            services.AddControllers().AddNewtonsoftJson();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                              builder =>
                              {
                                  builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                              });
            });
            // services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString));

      
            // services.AddTransient<MySqlConnection>(_ => new MySqlConnection(Configuration["ConnectionStrings:Default"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles(); 

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
