using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
	public class RefreshToken : BaseEntity
	{
		public string Token { get; set; }
		public DateTime Expires { get; set; }
		public string UserId { get; set; }
		public string RemoteIpAddress { get; set; }

		public User User { get; set; }

		public bool Active => DateTime.UtcNow <= Expires;
	}
}
