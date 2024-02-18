using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;

namespace abHelper
{
    public class ImageProcessing
    {

        public void CreateThumbnail(string imageFilePathWithFileName, string thumbnailFilePathWithFileName, int maximumWidth = 0, int maximumHeight = 0, bool isStretch = false, int quality = 80)
        {
            Bitmap loBMP = new Bitmap(imageFilePathWithFileName);
            ImageFormat loFormat = loBMP.RawFormat;

            int lnNewWidth = 0;
            int lnNewHeight = 0;
            decimal lnRatio;
            decimal lnTemp;

            if (isStretch == true)
            {
                lnNewWidth = maximumWidth;
                lnNewHeight = maximumHeight;
            }
            // If the image thumb height-width not specified OR image is smaller than a thumbnail
            else if ((maximumWidth == 0 && maximumHeight == 0) || (loBMP.Width < maximumWidth && loBMP.Height < maximumHeight))
            {
                lnNewWidth = loBMP.Width;
                lnNewHeight = loBMP.Height;
                if (quality == 80)
                {
                    quality = 100;
                }
            }
            // If the image thumb height not specified
            else if (maximumWidth > 0 && maximumHeight == 0)
            {
                lnRatio = (decimal)maximumWidth / loBMP.Width;
                lnNewWidth = maximumWidth;
                lnTemp = loBMP.Height * lnRatio;
                lnNewHeight = (int)lnTemp;
            }
            // If the image thumb width not specified
            else if (maximumWidth == 0 && maximumHeight > 0)
            {
                lnRatio = (decimal)maximumHeight / loBMP.Height;
                lnNewHeight = maximumHeight;
                lnTemp = loBMP.Width * lnRatio;
                lnNewWidth = (int)lnTemp;
            }
            else
            {
                lnRatio = (decimal)maximumWidth / loBMP.Width;
                lnNewWidth = maximumWidth;
                lnTemp = loBMP.Height * lnRatio;
                lnNewHeight = (int)lnTemp;

                if (lnNewHeight > maximumHeight)
                {
                    lnRatio = (decimal)maximumHeight / loBMP.Height;
                    lnNewHeight = maximumHeight;
                    lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
            }

            Bitmap bmpOut = new Bitmap(lnNewWidth, lnNewHeight);

            // Draws the image in the specified size with quality mode set to HighQuality
            Graphics g = Graphics.FromImage(bmpOut);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);
            g.Dispose();

            // Get an ImageCodecInfo object that represents the image codec.
            ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(loFormat);

            // Create an Encoder object for the Quality parameter.
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;

            bmpOut.Save(thumbnailFilePathWithFileName, imageCodecInfo, encoderParameters);
            bmpOut.Dispose();

            loBMP.Dispose();
        }

        public Image ChangeOpacity(Image image, float opacityvalue, int maximumWidth = 0, int maximumHeight = 0, bool isStretch = false)
        {
            int lnNewWidth = 0;
            int lnNewHeight = 0;
            decimal lnRatio;
            decimal lnTemp;

            if (isStretch == true)
            {
                lnNewWidth = maximumWidth;
                lnNewHeight = maximumHeight;
            }
            // If the image thumb height-width not specified OR image is smaller than a thumbnail
            else if ((maximumWidth == 0 && maximumHeight == 0) || (image.Width < maximumWidth && image.Height < maximumHeight))
            {
                lnNewWidth = image.Width;
                lnNewHeight = image.Height;
            }
            // If the image thumb height not specified
            else if (maximumWidth > 0 && maximumHeight == 0)
            {
                lnRatio = (decimal)maximumWidth / image.Width;
                lnNewWidth = maximumWidth;
                lnTemp = image.Height * lnRatio;
                lnNewHeight = (int)lnTemp;
            }
            // If the image thumb width not specified
            else if (maximumWidth == 0 && maximumHeight > 0)
            {
                lnRatio = (decimal)maximumHeight / image.Height;
                lnNewHeight = maximumHeight;
                lnTemp = image.Width * lnRatio;
                lnNewWidth = (int)lnTemp;
            }
            else
            {
                lnRatio = (decimal)maximumWidth / image.Width;
                lnNewWidth = maximumWidth;
                lnTemp = image.Height * lnRatio;
                lnNewHeight = (int)lnTemp;

                if (lnNewHeight > maximumHeight)
                {
                    lnRatio = (decimal)maximumHeight / image.Height;
                    lnNewHeight = maximumHeight;
                    lnTemp = image.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
            }

            Bitmap bmp = new Bitmap(lnNewWidth, lnNewHeight); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        public byte[] ConvertImageToByteArray(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            return ms.ToArray();
        }

        public Image ConvertByteArrayToImage(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// Method to get encoder infor for given image format.
        /// </summary>
        /// <param name="format">Image format</param>
        /// <returns>image codec info.</returns>
        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }
    }
}
