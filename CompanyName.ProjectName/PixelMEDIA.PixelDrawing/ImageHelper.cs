using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using PixelMEDIA.PixelCore.Helpers;

namespace PixelDrawing
{
    /// <summary>
    /// Helper functions for image manipulation.
    /// </summary>
	public static class ImageHelper
	{
		/// <summary>
		/// Loads an image from disk. Please wrap this call in a using statement to properly free up resources.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Image LoadImage(string path)
		{
			return Image.FromFile(path);
		}

		/// <summary>
		/// Rotates an image 90 degrees clockwise or counterclockwise.
		/// </summary>
		/// <param name="img"></param>
		/// <param name="counterClockwise"></param>
		public static void RotateImage90(Image img, bool counterClockwise)
		{
			var rotation = counterClockwise ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone;
			img.RotateFlip(rotation);
		}

		/// <summary>
		/// Rotates an image on disk 90 degrees clockwise or counterclockwise.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="counterClockwise"></param>
		public static void RotateImageFileOnDisk(string path, bool counterClockwise)
		{
			string tempPath;

			using (var img = LoadImage(path))
			{
				RotateImage90(img, counterClockwise);

				var tempStub = RandomHelper.GetRandomAlphanumericString(16);

				tempPath = SystemHelper.GetSimilarlyExtensionedNeighborFilePath(path, tempStub); //Can't save image on top of itself

				img.Save(tempPath);
			}

			SystemHelper.ReplaceFile(tempPath, path);
		}

        /// <summary>
        /// Crop an image to the provide dimensions.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="croppingRect"></param>
        /// <returns></returns>
		public static Image CropImage(Image img, Rectangle croppingRect)
		{
			Bitmap target = new Bitmap(croppingRect.Width, croppingRect.Height);

			using (Graphics g = Graphics.FromImage(target))
			{
				g.DrawImage(img, new Rectangle(0, 0, croppingRect.Width, croppingRect.Height), croppingRect, GraphicsUnit.Pixel);
			}

			return target;
		}

        /// <summary>
        /// Crop an image on disk.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="croppingRect"></param>
		public static void CropImageFileOnDisk(string path, Rectangle croppingRect)
		{
			string tempPath;

			using (var img = LoadImage(path))
			{
				using (var croppedImage = CropImage(img, croppingRect))
				{
					var tempStub = RandomHelper.GetRandomAlphanumericString(16);

					tempPath = SystemHelper.GetSimilarlyExtensionedNeighborFilePath(path, tempStub); //Can't save image on top of itself

					croppedImage.Save(tempPath);
				}
			}

			SystemHelper.ReplaceFile(tempPath, path);
		}
	}
}
