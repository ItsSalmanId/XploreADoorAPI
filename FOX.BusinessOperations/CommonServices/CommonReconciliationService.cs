using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Reconciliation;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using SautinSoft;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.CommonServices
{
    public class CommonReconciliationService
    {
        private readonly DBContextReconciliations _reconciliationCPContext = new DBContextReconciliations();
        private readonly GenericRepository<ReconciliationFiles> _reconciliationFilesRepository;

        public CommonReconciliationService()
        {
            _reconciliationFilesRepository = new GenericRepository<ReconciliationFiles>(_reconciliationCPContext);
        }

        public int tifToImage(UserProfile profile, string orgFilePath, string imgServerPath, string imgDirPath, long reconciliation_ID, string reconciliationCategory, int pageCounter, out int pageCounterOut)
        {
            if (!Directory.Exists(imgServerPath))
                Directory.CreateDirectory(imgServerPath);

            var imgName = reconciliation_ID + "_" + pageCounter + DateTime.Now.ToString("ddMMyyyHHmmssffff") + ".jpg";
            var imgPathServer = imgServerPath + "\\" + imgName;
            var imgPathInDB = imgDirPath + "\\" + imgName;

            var logoName = "Logo_" + imgName + ".jpg";
            var logoPathServer = imgServerPath + "\\" + logoName;
            var logoPathInDB = imgDirPath + "\\" + logoName;

            var imgCount = CountTiffFileImages(orgFilePath);

            for (int i = 0; i < imgCount; i++)
            {
                var bitmap = ExtractImageFromTiffFile(orgFilePath, i, logoPathServer);

                // Get an ImageCodecInfo object that represents the JPEG codec.
                ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);

                // Create an Encoder object for the Quality parameter.
                Encoder encoder = Encoder.Quality;

                // Create an EncoderParameters object. 
                EncoderParameters encoderParameters = new EncoderParameters(1);

                // Save the image as a JPEG file with quality level.
                var size = bitmap.Size.Width * bitmap.Size.Height;
                var quality = 100L;
                if (size >= 20000000)//if size is > than 20 MB, compress it to reduce size
                {
                    quality = 60L;
                }
                else if (size >= 10000000)//10 MB
                {
                    quality = 70L;
                }
                else if (size >= 5000000)//5 MB
                {
                    quality = 80L;
                }

                EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                bitmap.Save(imgPathServer, imageCodecInfo, encoderParameters);

                var imgPath = imgPathInDB;
                var logoImgPath = logoPathInDB;
                AddFilesToDatabase(imgPath, reconciliation_ID, logoImgPath, reconciliationCategory, profile);
            }
            pageCounterOut = pageCounter;
            return imgCount;
        }

        public Bitmap ExtractImageFromTiffFile(string orgImgPath, int curPageNo, string logoPath)
        {
            using (FileStream fs = new FileStream(orgImgPath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (Image img = Image.FromStream(fs, true, false))
                {
                    FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
                    img.SelectActiveFrame(dimention, curPageNo);
                    Bitmap bmp = new Bitmap(img);
                    SaveWithNewDimention(bmp, 115, 150, 100, logoPath);
                    img.Dispose();
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
                    FrameDimension dimention = new FrameDimension(currentImg.FrameDimensionsList[0]);
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
        }

        private void AddFilesToDatabase(string filePath, long reconciliation_Id, string logoPath, string category, UserProfile profile)
        {
            try
            {
                //ReconciliationFiles originalQueueFiles = _reconciliation.GetFirst(t => t.RECONCILIATION_FILE_ID == workId && !t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));

                //if (originalQueueFiles == null)
                //{
                //    //If Work Order files is deleted
                //var reconciliationFiles = _reconciliationFilesRepository.Get(t => t.WORK_ID == workId && t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));
                //if (originalQueueFiles == null)
                //{
                var reconciliationFile = new ReconciliationFiles();

                reconciliationFile.RECONCILIATION_FILE_ID = Helper.getMaximumId("FOX_RECONCILIATION_FILE_ID");
                reconciliationFile.RECONCILIATION_ID = reconciliation_Id;
                reconciliationFile.RECONCILIATION_CATEGORY = category;

                reconciliationFile.IMAGE_PATH = filePath;
                reconciliationFile.LOGO_PATH = logoPath;
                reconciliationFile.CREATED_BY = reconciliationFile.MODIFIED_BY = profile.UserName;
                reconciliationFile.CREATED_DATE = reconciliationFile.MODIFIED_DATE = DateTime.Now;
                reconciliationFile.DELETED = false;
                _reconciliationFilesRepository.Insert(reconciliationFile);
                _reconciliationFilesRepository.Save();
                //    }
                //}
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

        public int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
        }

        public void SavePdfToImages(UserProfile profile, string orgPdfPath, long reconciliation_ID, string reconciliationCategory, int noOfPages, int pageCounter, out int pageCounterOut)
        {
            var originalFileDirectory = AppConfiguration.ReconciliationOriginalFilesDirectory;
            var imgDirPath = AppConfiguration.ReconciliationConvertedImagesDirectory;
            var originalFilePathServer = HttpContext.Current.Server.MapPath("~/" + originalFileDirectory + "\"");
            var imgServerPath = HttpContext.Current.Server.MapPath("~/" + imgDirPath + "\"");

            var imgName = reconciliation_ID + "_" + pageCounter + DateTime.Now.ToString("ddMMyyyHHmmssffff") + ".jpg";
            var imgPathServer = imgServerPath + "\\" + imgName;
            var imgPathInDB = imgDirPath + "\\" + imgName;

            var logoName = "Logo_" + imgName + ".jpg";
            var logoPathServer = imgServerPath + "\\" + logoName;
            var logoPathInDB = imgDirPath + "\\" + logoName;


            if (!Directory.Exists(imgServerPath))
            {
                Directory.CreateDirectory(imgServerPath);
            }

            if (File.Exists(orgPdfPath))
            {
                for (int i = 0; i < noOfPages; i++, pageCounter++)
                {
                    Image img;
                    PdfFocus f = new PdfFocus();
                    //f.Serial = "10261435399";
                    f.Serial = "80033727929";
                    f.OpenPdf(orgPdfPath);

                    if (f.PageCount > 0)
                    {
                        //Save all PDF pages to jpeg images
                        f.ImageOptions.Dpi = 120;
                        f.ImageOptions.ImageFormat = ImageFormat.Jpeg;

                        var image = f.ToImage(i + 1);
                        //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
                        using (var ms = new MemoryStream(image))
                        {
                            img = Image.FromStream(ms);
                            img.Save(imgPathServer, ImageFormat.Jpeg);
                            Bitmap bmp = new Bitmap(img);
                            ConvertPDFToImages ctp = new ConvertPDFToImages();
                            ctp.SaveWithNewDimention(bmp, 115, 150, 100, logoPathServer);
                        }
                    }
                    //End
                    var imgPath = imgPathInDB;
                    var logoImgPath = logoPathInDB;
                    AddFilesToDatabase(imgPath, reconciliation_ID, logoImgPath, reconciliationCategory, profile);
                }
            }
            pageCounterOut = pageCounter;
        }
    }
}
