using ApplicationCore.Helpers;
using ApplicationCore.Models;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;


namespace ApplicationCore.Specifications
{
	public class AttachmentFilterSpecifications : BaseSpecification<UploadFile>
	{
		public AttachmentFilterSpecifications()
			: base(item => !item.Removed)
		{

		}


		public AttachmentFilterSpecifications(PostType postType)
			: base(item => !item.Removed && item.PostType == postType)
		{

		}


		public AttachmentFilterSpecifications(PostType postType, int postId)
			: base(item => !item.Removed && item.PostType == postType && item.PostId == postId)
		{

		}

		public AttachmentFilterSpecifications(IList<int> ids) : base(item => !item.Removed && ids.Contains(item.Id))
		{

		}

		public AttachmentFilterSpecifications(ICollection<PostType> postTypes)
			: base(item => !item.Removed & postTypes.Contains(item.PostType))
		{

		}
	}

}
