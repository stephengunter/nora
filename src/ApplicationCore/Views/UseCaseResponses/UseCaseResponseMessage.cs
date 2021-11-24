using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Views
{
	public abstract class UseCaseResponseMessage
	{
		public bool Success { get; }
		public string Message { get; }

		protected UseCaseResponseMessage(bool success = false, string message = null)
		{
			this.Success = success;
			this.Message = message;
		}
	}
}
