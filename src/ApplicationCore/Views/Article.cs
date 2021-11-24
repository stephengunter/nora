using Infrastructure.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Views
{
    public class ArticleViewModel : BaseRecordView
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Date { get; set; }
    }
}
