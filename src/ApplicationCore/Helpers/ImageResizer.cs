using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ApplicationCore.Helpers
{
	public enum ImageResizeType
	{
		Scale = 0,
		Croped = 1,
		Unknown = -1
	}

	public class ImageResizer
	{
		public static Image GetResizedImage(Image imgSource, int width, int height)
		{
			// 依比例縮圖
			// 計算大小
			if (width <= imgSource.Width && height >= (1.0 * imgSource.Height * width / imgSource.Width))
			{
				height = (int)(1.0 * imgSource.Height * width / imgSource.Width);
			}
			else if (height <= imgSource.Height && width >= (1.0 * imgSource.Width * height / imgSource.Height))
			{
				width = (int)(1.0 * imgSource.Width * height / imgSource.Height);
			}

			// 如果需要縮圖
			if (width < imgSource.Width || height < imgSource.Height)
			{
				return ScaleByFixedSize(imgSource, width, height);
			}

			return null;

		}

		public static Image GetCropedImage(Image imgSource, int width, int height)
		{

			bool isSizeChanged = false;
			// 計算截圖範圍
			int cropWidth = imgSource.Width;
			int cropHeight = imgSource.Height;

			if (width < imgSource.Width && height < imgSource.Height)
			{
				if (1.0 * imgSource.Width / width * height <= imgSource.Height)
				{
					cropHeight = (int)(1.0 * imgSource.Width / width * height);
					isSizeChanged = true;
				}
				else if (1.0 * imgSource.Height / height * width <= imgSource.Width)
				{
					cropWidth = (int)(1.0 * imgSource.Height / height * width);
					isSizeChanged = true;
				}
			}
			else if (width < imgSource.Width && height >= imgSource.Height)
			{
				cropWidth = (int)(1.0 * imgSource.Height / height * width);
				isSizeChanged = true;
			}
			else if (width >= imgSource.Width && height < imgSource.Height)
			{
				cropHeight = (int)(1.0 * imgSource.Width / width * height);
				isSizeChanged = true;
			}
			else
			{
				// 原圖
			}

			// 進行截圖及縮圖
			if (isSizeChanged)
			{
				var cropedImage = Crop(imgSource, cropWidth, cropHeight, AnchorPosition.Center);
				return ScaleByFixedSize(cropedImage, width, height);
			}


			return null;


		}

		public static Image ScaleByPercent(Image imgPhoto, int Percent)
		{

			float nPercent = ((float)Percent / 100);

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = 0;
			int destY = 0;
			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
									 PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
									imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

		public static Image ScaleByFixedSize(Image imgPhoto, int Width, int Height)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width / (float)sourceWidth);
			nPercentH = ((float)Height / (float)sourceHeight);
			if (nPercentH < nPercentW)
			{
				nPercent = nPercentH;
				destX = System.Convert.ToInt16((Width -
							  (sourceWidth * nPercent)) / 2);
			}
			else
			{
				nPercent = nPercentW;
				destY = System.Convert.ToInt16((Height -
							  (sourceHeight * nPercent)) / 2);
			}

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height,
							  PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
							 imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(System.Drawing.Color.White);
			grPhoto.InterpolationMode =
					InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}



		public static Image Crop(Image imgPhoto, int Width,
							 int Height, AnchorPosition Anchor)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width / (float)sourceWidth);
			nPercentH = ((float)Height / (float)sourceHeight);

			if (nPercentH < nPercentW)
			{
				nPercent = nPercentW;
				switch (Anchor)
				{
					case AnchorPosition.Top:
						destY = 0;
						break;
					case AnchorPosition.Bottom:
						destY = (int)
							(Height - (sourceHeight * nPercent));
						break;
					default:
						destY = (int)
							((Height - (sourceHeight * nPercent)) / 2);
						break;
				}
			}
			else
			{
				nPercent = nPercentH;
				switch (Anchor)
				{
					case AnchorPosition.Left:
						destX = 0;
						break;
					case AnchorPosition.Right:
						destX = (int)
						  (Width - (sourceWidth * nPercent));
						break;
					default:
						destX = (int)
						  ((Width - (sourceWidth * nPercent)) / 2);
						break;
				}
			}

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width,
					Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
					imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode =
					InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

		public static Image Base64ToImage(string img)
		{
			Image result = null;
			byte[] bytes = Convert.FromBase64String(img);
			using (MemoryStream ms = new MemoryStream(bytes))
			{
				result = Image.FromStream(ms);
			}
			return result;
		}

		public static string ImageToBase64(Image img, ImageFormat format)
		{
			string result = string.Empty;

			using (MemoryStream ms = new MemoryStream())
			{
				// Convert Image to byte[]
				img.Save(ms, format);
				byte[] imageBytes = ms.ToArray();

				// Convert byte[] to Base64 String
				result = Convert.ToBase64String(imageBytes);
			}

			return result;
		}

	}

	public enum AnchorPosition
	{
		Top = 0,
		Bottom = 1,
		Center = 2,
		Left = 3,
		Right = 4
	}

}
