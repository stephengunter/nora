using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DataAccess
{
	public class AppDBSeed
	{
		public static async Task EnsureSeedData(IServiceProvider serviceProvider)
		{
			Console.WriteLine("Seeding database...");

			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var defaultContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
				defaultContext.Database.Migrate();

				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				await SeedRoles(roleManager);
				await SeedUsers(userManager);

			}

			Console.WriteLine("Done seeding database.");
			Console.WriteLine();
		}

		static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			var roles = new List<string> { AppRoles.Dev.ToString(), AppRoles.Boss.ToString() };
			foreach (var item in roles)
			{
				await AddRoleIfNotExist(roleManager, item);
			}


		}

		static async Task AddRoleIfNotExist(RoleManager<IdentityRole> roleManager, string roleName)
		{
			var role = await roleManager.FindByNameAsync(roleName);
			if (role == null)
			{
				await roleManager.CreateAsync(new IdentityRole { Name = roleName });

			}


		}

		static async Task SeedUsers(UserManager<User> userManager)
		{
			await CreateUserIfNotExist(userManager, new User {
				Name = "Stephen",
				Email = "traders.com.tw@gmail.com",
				UserName = "traders.com.tw@gmail.com",
				
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString()

			}, new List<string>() { AppRoles.Dev.ToString() });

			await CreateUserIfNotExist(userManager, new User
			{
				Name = "Nora",
				Email = "nora@gmail.com",
				UserName = "nora@gmail.com",
				
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString()
			});

		}


		static async Task CreateUserIfNotExist(UserManager<User> userManager, string email, IList<string> roles = null)
		{
			bool isAdmin = false;
			if (!roles.IsNullOrEmpty())
			{
				isAdmin = roles.Select(r => r.EqualTo(AppRoles.Dev.ToString()) || r.EqualTo(AppRoles.Boss.ToString())).FirstOrDefault();
			}

			var newUser = new User
			{
				Email = email,
				UserName = email,


				EmailConfirmed = isAdmin,
				SecurityStamp = Guid.NewGuid().ToString()

			};

			await CreateUserIfNotExist(userManager, newUser, roles);

		}

		static async Task CreateUserIfNotExist(UserManager<User> userManager, User user, IList<string> roles = null)
		{
			var existingUser = await userManager.FindByEmailAsync(user.Email);
			if (existingUser == null)
			{
				var result = await userManager.CreateAsync(user);

				if (!roles.IsNullOrEmpty())
				{
					await userManager.AddToRolesAsync(user, roles);
				}


			}
			else
			{
				if (!roles.IsNullOrEmpty())
				{
					foreach (var role in roles)
					{
						bool hasRole = await userManager.IsInRoleAsync(existingUser, role);
						if (!hasRole) await userManager.AddToRoleAsync(existingUser, role);
					}
				}


			}
		}



	}
}
