using GeekShopping.Email.MessageConsumer;
using GeekShopping.Email.Model.Context;
using GeekShopping.Email.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GeekShopping.Email
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			// Add services to the container.
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			var dbContextBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			dbContextBuilder.UseSqlServer(connectionString);
			builder.Services.AddSingleton(new EmailRepository(dbContextBuilder.Options));
			builder.Services.AddScoped<IEmailRepository, EmailRepository>()
				            .AddScoped<IDataService, DataService>();

			builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

			builder.Services.AddControllers();
			builder.Services.AddAuthentication("Bearer")
							.AddJwtBearer("Bearer", options =>
							{
								options.Authority = "https://localhost:4435/";
								options.TokenValidationParameters = new TokenValidationParameters
								{
									ValidateAudience = false
								};
							});

			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("ApiScope", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireClaim("scope", "geek_shopping");
				});
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.EmailAPI", Version = "v1" });
				//c.EnableAnnotations();
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = @"Enter 'Bearer' [space] and your token!",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In= ParameterLocation.Header
			},
			new List<string> ()
		}
	});
			});

			var app = builder.Build();
			app.Services.CreateScope().ServiceProvider.GetService<IDataService>().InicializaDB();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
