using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ApplicationCore.Helpers
{
	public static class PhotoHelpers
    {
		public static Image Resize(this Image imgSource, ImageResizeType type, int width, int height)
		{ 
			if(type == ImageResizeType.Scale) return ImageResizer.GetResizedImage(imgSource, width, height);
			if(type == ImageResizeType.Croped) return ImageResizer.GetCropedImage(imgSource, width, height);
			return null;
		}

		public static ImageResizeType ToImageResizeType(this string val)
		{
			try
			{
				var type = val.ToEnum<ImageResizeType>();
				return type;
			}
			catch (Exception)
			{
				return ImageResizeType.Unknown;
			}
		}
	}
}
