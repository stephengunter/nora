using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Models
{
	public class User : IdentityUser, IAggregateRoot
	{
		public string Name { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public ICollection<OAuth> OAuthList { get; set; }

		public RefreshToken RefreshToken { get; set; }

	}
}
