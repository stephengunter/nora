using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ApplicationCore.Helpers;

namespace ApplicationCore.Views
{
	public class AdminRequest
	{
		public string Key { get; set; }
		public string Cmd { get; set; }

		public string Data { get; set; } //json string
	}

	public class AdminFileRequest : AdminRequest
	{
		
		public List<IFormFile> Files { get; set; } = new List<IFormFile>();


		public IFormFile GetFile(string name)
		{
			if (Files.IsNullOrEmpty()) return null;
			return Files.FirstOrDefault(item => Path.GetFileNameWithoutExtension(item.FileName) == name);

		}
	}
}
