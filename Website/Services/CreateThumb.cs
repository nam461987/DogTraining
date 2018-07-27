using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Website.Services
{
    public class CreateThumb
    {
        enum Dimensions
        {
            Width,
            Height
        }
        enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }
        [STAThread]
        /// <summary>
        /// <para>
        /// args[0]: ten link
        /// args[1]: folder tao thumbnail
        /// args[2]: height
        /// args[3]: width
        /// args[4]: percent
        /// args[5]: kieu resize
        /// </para>
        /// </summary>
        public static string ResizeImage(string[] args)
        {
            //set a working directory
            var linkSplit = args[0].Split('/');
            string WorkingDirectory = args[0].Replace(linkSplit[linkSplit.Length - 1], "");
            //string WorkingDirectory = linkSplit[3];

            var imageName = linkSplit[linkSplit.Length - 1].Split('.')[0];
            var imageTail = linkSplit[linkSplit.Length - 1].Split('.')[1];
            var folderName = args[1];

            var height = int.Parse(string.IsNullOrEmpty(args[2]) ? "10" : args[2]);
            var width = int.Parse(string.IsNullOrEmpty(args[3]) ? "10" : args[3]);
            var percent = int.Parse(string.IsNullOrEmpty(args[4]) ? "10" : args[4]);

            //create a image object containing a verticel photograph
            Image imgPhotoVert = Image.FromFile(HttpContext.Current.Server.MapPath("~" + WorkingDirectory + imageName + "." + imageTail));
            //Image imgPhotoHoriz = Image.FromFile(WorkingDirectory + @"\imageresize_horiz.jpg");
            Image imgPhoto = null;
            var result = "";
            if (args[5] == "1")
            {
                imgPhoto = ScaleByPercent(imgPhotoVert, percent);
                imgPhoto.Save(HttpContext.Current.Server.MapPath("~" + WorkingDirectory + folderName + @"\" + imageName + "_1." + imageTail));
                imgPhoto.Dispose();
                result = WorkingDirectory + folderName + @"/" + imageName + "_1." + imageTail;
            }
            else if (args[5] == "2")
            {
                imgPhoto = ConstrainProportions(imgPhotoVert, width, Dimensions.Width);
                imgPhoto.Save(HttpContext.Current.Server.MapPath("~" + WorkingDirectory + folderName + @"\" + imageName + "_2." + imageTail));
                imgPhoto.Dispose();
                result = WorkingDirectory + folderName + @"/" + imageName + "_2." + imageTail;
            }
            else if (args[5] == "3")
            {
                imgPhoto = FixedSize(imgPhotoVert, width, height);
                imgPhoto.Save(HttpContext.Current.Server.MapPath("~" + WorkingDirectory + folderName + @"\" + imageName + "_3." + imageTail));
                imgPhoto.Dispose();
                result = WorkingDirectory + folderName + @"/" + imageName + "_3." + imageTail;
            }
            else if (args[5] == "4")
            {
                imgPhoto = Crop(imgPhotoVert, width, height, AnchorPosition.Center);
                imgPhoto.Save(HttpContext.Current.Server.MapPath("~" + WorkingDirectory + folderName + @"\" + imageName + "-" + width + "x" + height + "." + imageTail));
                imgPhoto.Dispose();
                result = WorkingDirectory + folderName + @"/" + imageName + "-" + width + "x" + height + "." + imageTail;
            }
            //imgPhoto = Crop(imgPhotoHoriz, 200, 200, AnchorPosition.Center);
            //imgPhoto.Save(WorkingDirectory + @"\images\imageresize_5.jpg", ImageFormat.Jpeg);
            //imgPhoto.Dispose();

            return result;
        }
        static Image ScaleByPercent(Image imgPhoto, int Percent)
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

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            float nPercent = 0;

            switch (Dimension)
            {
                case Dimensions.Width:
                    nPercent = ((float)Size / (float)sourceWidth);
                    break;
                default:
                    nPercent = ((float)Size / (float)sourceHeight);
                    break;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
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

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
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
                        destY = (int)(Height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = (int)((Height - (sourceHeight * nPercent)) / 2);
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
                        destX = (int)(Width - (sourceWidth * nPercent));
                        break;
                    default:
                        destX = (int)((Width - (sourceWidth * nPercent)) / 2);
                        break;
                }
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}