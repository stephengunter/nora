using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Settings
{
	public class AppSettings
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Email { get; set; }
		public string ClientUrl { get; set; }
		public string AdminUrl { get; set; }
		public string BackendUrl { get; set; }

		public string UploadPath { get; set; }
		public string TemplatePath { get; set; }
	}

	public class AuthSettings
	{
		public string SecurityKey { get; set; }
		public int TokenValidHours { get; set; }
		public int RefreshTokenDaysToExpire { get; set; }

	}

	public class AdminSettings
	{
		public string Key { get; set; }
		public string Email { get; set; }
		public string Id { get; set; }
		public string BackupPath { get; set; }
		public string DataPath { get; set; }
	}
}
