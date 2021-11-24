using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
	public enum PostType
	{
		Option = 0,
		Resolve = 1,
		Note = 2,
		Manual = 3,
		Feature = 4,
		Emoji = 5,
		None = -1
	}

	public class UploadFile : BaseUploadFile
	{
		public PostType PostType { get; set; } = PostType.None;
		public int PostId { get; set; }

	}
}
