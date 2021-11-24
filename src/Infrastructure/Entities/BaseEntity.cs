using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
	public abstract class BaseEntity : IAggregateRoot
	{
		public int Id { get; set; }
	}
}
