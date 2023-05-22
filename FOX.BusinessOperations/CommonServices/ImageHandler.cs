using iTextSharp.text;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.CommonServices
{
    public class ImageHandler
    {
        /// <summary>
        /// Method to resize, convert and save the image.
        /// </summary>
        /// <param name="image">Bitmap image.</param>
        /// <param name="maxWidth">resize width.</param>
        /// <param name="maxHeight">resize height.</param>
        /// <param name="quality">quality setting value.</param>
        /// <param name="filePath">file path.</param>      
        public void Save(Bitmap image, int maxWidth, int maxHeight, int quality, string filePath)
        {
            // Get the image's original width and height
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            // Get an ImageCodecInfo object that represents the JPEG codec.
            ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Png);

            // Create an Encoder object for the Quality parameter.
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(filePath, imageCodecInfo, encoderParameters);
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

        public void ImagesToPdf(string[] imagepaths, string pdfpath)
        {
            //iTextSharp.text.Rectangle pageSize = null;

            //var currentPath = HttpContext.Current.Server.MapPath(@"~/" + imagepaths[0].ToString());
            //using (var srcImage = new Bitmap(currentPath))
            //{
            //    pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
            //}

            using (var ms = new MemoryStream())
            {
                //var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
                var document = new iTextSharp.text.Document(PageSize.A4, 0, 0, 0, 0);
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();
                int counter = 0;
                foreach (var currImgPath in imagepaths)
                {
                    //\\\\10.10.30.165\\FoxDocumentDirectory\\Fox\\1012714\\05-20-2023\\Images
                    var mappedPath = @"\\10.10.30.165\" + currImgPath.ToString();
                    //var mappedPath = "\\\\it-126" + '\\' + currImgPath.ToString(); if you want to run on qa uncomment this line and commment above line
                    if (File.Exists(mappedPath))
                    {
                        var image = iTextSharp.text.Image.GetInstance(mappedPath);
                        image.Alignment = Element.ALIGN_MIDDLE;
                        image.ScaleToFit(document.PageSize.Width - 10, document.PageSize.Height - 10);
                        image.SetAbsolutePosition((PageSize.A4.Width - image.ScaledWidth) / 2, (PageSize.A4.Height - image.ScaledHeight) / 2);
                        if (!document.Add(image))
                        {
                            throw new Exception("Unable to add image to page!");
                        }
                        //document.Add(image);
                        counter++;
                        if (counter < imagepaths.Length) {
                            document.NewPage();
                        }
                    }
                }
                document.Close();
                File.WriteAllBytes(pdfpath, ms.ToArray());
            }
        }

    }
}