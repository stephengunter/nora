using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Helpers;
using Infrastructure.Entities;

namespace ApplicationCore.Models
{
    public class Article : BaseRecord
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Date => CreatedAt.ToDateString();
    }
}
