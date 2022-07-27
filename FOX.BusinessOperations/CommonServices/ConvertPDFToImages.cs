using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.OriginalQueueModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
//using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.CommonService
{
    class ConvertPDFToImages
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        //private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFiles;
        public ConvertPDFToImages()
        {
            //_QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _OriginalQueueFiles = new GenericRepository<OriginalQueueFiles>(_QueueContext);
        }

        //public int PDFToImages(string pdfPath, string imagePath, long WORK_ID)
        //{
        //    var pdfToImg = new NReco.PdfRenderer.PdfToImageConverter();
        //    pdfToImg.ScaleTo = 200; // fit 200x200 box
        //    if (!Directory.Exists(imagePath + WORK_ID))
        //        Directory.CreateDirectory(imagePath + WORK_ID);
        //    string[] a = pdfToImg.GenerateImages(pdfPath, NReco.PdfRenderer.ImageFormat.Jpeg, imagePath + WORK_ID);


        //    return a.Length;

        //    //int totalPages = 0;
        //    //SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
        //    //f.OpenPdf(pdfPath);
        //    //if (f.PageCount > 0)
        //    //{
        //    //    //Save all PDF pages to jpeg images and put them in ArrayList, set 120 dpi
        //    //    f.ImageOptions.Dpi = 200;
        //    //    f.ImageOptions.ImageFormat = ImageFormat.Jpeg;
        //    //    ArrayList aa = f.ToImage();
        //    //    int i = 0;
        //    //    foreach (var item in aa)
        //    //    {
        //    //        totalPages++;
        //    //        if (Directory.Exists(imagePath))
        //    //            Directory.CreateDirectory(imagePath);
        //    //        FileStream fs = new FileStream(imagePath + WORK_ID + "_" + totalPages + ".jpg", FileMode.Create, FileAccess.Write);
        //    //        fs.Write(item as byte[], 0,  (item as byte[]).Length);
        //    //        totalPages++;
        //    //    }

        //    //}
        //    //return totalPages;
        //}

        public int tifToImage(string tifImagePath, string imagePath, long workId, long pageCounter, string ImgDirPath, out long pageCounterOut, bool _isStoreToDB)
        {
            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);
            var imgCount = CountTiffFileImages(tifImagePath);
            for (int i = 0; i < imgCount; i++, pageCounter++)
            {
                using (
                var bitmap = ExtractImageFromTiffFile(tifImagePath, i, imagePath + "\\Logo_" + workId + "_" + pageCounter + ".jpg"))
                {

                    // Get an ImageCodecInfo object that represents the JPEG codec.
                    //ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);

                    // Create an Encoder object for the Quality parameter.
                    //System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

                    // Create an EncoderParameters object. 
                    //EncoderParameters encoderParameters = new EncoderParameters(1);
                   
                    // Save the image as a JPEG file with quality level.
                    var size = bitmap.Size.Width * bitmap.Size.Height;
                    //var quality = 100L;
                    //if (size >= 20000000)//if size is > than 20 MB, compress it to reduce size
                    //{
                    //    quality = 60L;
                    //}
                    //else if (size >= 10000000)//10 MB
                    //{
                    //    quality = 70L;
                    //}
                    //else if (size >= 5000000)//5 MB
                    //{
                    //    quality = 80L;
                    //}

                    //EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                    //encoderParameters.Param[0] = encoderParameter;
                    Bitmap bmp = new Bitmap(bitmap);
                    //encoderParameter.Dispose();

                    if (_isStoreToDB == false)
                    {
                        bitmap.Dispose();
                        bmp.Save(imagePath + "\\" + workId + "_" + pageCounter + "_" + i + ".jpg", ImageFormat.Jpeg);
                    }
                    if (_isStoreToDB == true)
                    {
                        bitmap.Dispose();
                        bmp.Save(imagePath + "\\" + workId + "_" + pageCounter + ".jpg", ImageFormat.Jpeg);
                        var imgPath = ImgDirPath + "\\" + workId + "_" + pageCounter + ".jpg";
                        var logoImgPath = ImgDirPath + "\\Logo_" + workId + "_" + pageCounter + ".jpg";
                        AddFilesToDatabase(imgPath, workId, logoImgPath);
                    }
                    bmp.Dispose();
                   // encoderParameters.Dispose();
                }
            }
            pageCounterOut = pageCounter;
            return imgCount;
        }

        public Bitmap ExtractImageFromTiffFile(string imagePath, int curPageNo, string logoPath)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (Image img = Image.FromStream(fs, true, false))
                {
                    FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
                    img.SelectActiveFrame(dimention, curPageNo);            
                    Bitmap bmp = new Bitmap(img);
                    img.Dispose();
                    SaveWithNewDimention(bmp, 115, 150, 100, logoPath);                   
                    return bmp;
                }


            }
        }

        public int CountTiffFileImages(string imagePath)
        {
            //System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
            int pages = 0;
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (Image currentImg = Image.FromStream(fs, true, false))
                {
                    System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(currentImg.FrameDimensionsList[0]);
                    pages = currentImg.GetFrameCount(dimention);
                    currentImg.Dispose();
                }
            }
            return pages;
        }


        /// <summary>
        /// Method to resize, convert and save the image.
        /// </summary>
        /// <param name="image">Bitmap image.</param>
        /// <param name="maxWidth">resize width.</param>
        /// <param name="maxHeight">resize height.</param>
        /// <param name="quality">quality setting value.</param>
        /// <param name="filePath">file path.</param>      
        public void SaveWithNewDimention(Bitmap image, int maxWidth, int maxHeight, int quality, string filePath)
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
            if (newHeight == 0) { newHeight = 1; }
            if (newWidth == 0) { newWidth = 1; }

            // Convert other formats (including CMYK) to RGB.
            using (Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb))
            {
                // Draws the image in the specified size with quality mode set to HighQuality
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                
                // Get an ImageCodecInfo object that represents the JPEG codec.
                //ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Png);

                // Create an Encoder object for the Quality parameter.
                //Encoder encoder = Encoder.Quality;

                // Create an EncoderParameters object. 
                //EncoderParameters encoderParameters = new EncoderParameters(1);

                // Save the image as a JPEG file with quality level.
                //EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                //encoderParameters.Param[0] = encoderParameter;
                Bitmap cimage =new Bitmap(newImage);
                newImage.Dispose();
                cimage.Save(filePath,ImageFormat.Jpeg);
                //encoderParameter.Dispose();                           
                //encoderParameters.Dispose();
                
            }          
        }

        private void AddFilesToDatabase(string filePath, long workId, string logoPath)
        {

            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxConnectionString"].ToString()))
            //{
            //    try
            //    {
            //        var dbname = conn.Database;
            //        SqlCommand cmd = new SqlCommand("[FOX_PROC_INSERT_WORK_FILES_FROM_SERVICE]", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        var fileid = Helper.getMaximumId("FOXREHAB_FILE_ID");
            //        cmd.Parameters.AddWithValue("@FILE_ID", fileid);
            //        cmd.Parameters.AddWithValue("@WORK_ID", workId);
            //        cmd.Parameters.AddWithValue("@UNIQUE_ID", workId.ToString());
            //        cmd.Parameters.AddWithValue("@FILE_PATH1", filePath);
            //        cmd.Parameters.AddWithValue("@FILE_PATH", logoPath);
            //        conn.Open();
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}


            try
            {
                OriginalQueueFiles originalQueueFiles = _OriginalQueueFiles.GetFirst(t => t.WORK_ID == workId && !t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));

                if (originalQueueFiles == null)
                {
                    //If Work Order files is deleted
                   originalQueueFiles = _OriginalQueueFiles.GetFirst(t => t.WORK_ID == workId && t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));
                    if (originalQueueFiles == null)
                    {
                        originalQueueFiles = new OriginalQueueFiles();

                        originalQueueFiles.FILE_ID = Helper.getMaximumId("FOXREHAB_FILE_ID");
                        originalQueueFiles.WORK_ID = workId;
                        originalQueueFiles.UNIQUE_ID = workId.ToString();
                        originalQueueFiles.FILE_PATH1 = filePath;
                        originalQueueFiles.FILE_PATH = logoPath;
                        originalQueueFiles.deleted = false;

                        _OriginalQueueFiles.Insert(originalQueueFiles);
                        _OriginalQueueFiles.Save();
                   }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
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
