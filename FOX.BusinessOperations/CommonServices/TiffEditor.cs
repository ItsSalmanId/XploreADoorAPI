using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FOX.BusinessOperations.CommonService
{
    public class TiffEditor
    {


        public TiffEditor()
        {
            //
            // TODO: Add constructor logic here
            //
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
        //for tif
        public Bitmap ExtractImageFromTiffFile_tif(Image img, int curPageNo)
        {
            //using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            //{
            //    using (Image photo = Image.FromStream(fs, true, false))
            //    {
            //        //tifObj.generateThumbNails(System.Drawing.Image.FromFile(path), thumbNailDirPath, 90);
            //        tifObj.generateThumbNails(photo, thumbNailDirPath, 90);
            //    }
            //}

            //System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
            System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
            img.SelectActiveFrame(dimention, curPageNo);
            Bitmap bmp = new Bitmap(img);
            img.Dispose();
            return bmp;

            //previuos code
            //System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
            //System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
            //img.SelectActiveFrame(dimention, curPageNo);
            //Bitmap bmp = new Bitmap(img);
            //img.Dispose();
            //return bmp;
        }

        //
        public Bitmap ExtractImageFromTiffFile(string imagePath, int curPageNo)
        {
            //using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            //{
            //    using (Image photo = Image.FromStream(fs, true, false))
            //    {
            //        //tifObj.generateThumbNails(System.Drawing.Image.FromFile(path), thumbNailDirPath, 90);
            //        tifObj.generateThumbNails(photo, thumbNailDirPath, 90);
            //    }
            //}
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (Image img = Image.FromStream(fs, true, false))
                {
                    //System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
                    System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
                    img.SelectActiveFrame(dimention, curPageNo);
                    Bitmap bmp = new Bitmap(img);
                    img.Dispose();
                    return bmp;
                }


            }
            //previuos code
            //System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
            //System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
            //img.SelectActiveFrame(dimention, curPageNo);
            //Bitmap bmp = new Bitmap(img);
            //img.Dispose();
            //return bmp;
        }
        public List<Bitmap> GetListOfImagesFromTiffFile(string imagePath)
        { //using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            List<Bitmap> btmp = new List<Bitmap>();
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite))
            {
                int TotalPages = 0;
                using (Image img = Image.FromStream(fs, true, false))
                {
                    TotalPages = TiffEditor.totalPages(img);
                }

                for (int i = 0; i < TotalPages; i++)
                {
                    using (Image img = Image.FromStream(fs, true, false))
                    {
                        System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
                        img.SelectActiveFrame(dimention, i);
                        Bitmap bmp = new Bitmap(img);
                        img.Dispose();
                        btmp.Add(bmp);
                    }
                }
            }
            return btmp;
        }


        public void SaveMultiPageTiff(System.Drawing.Image[] img, string pathToSave)
        {
            System.Drawing.Image pages = null;
            System.Drawing.Imaging.Encoder enc = System.Drawing.Imaging.Encoder.SaveFlag;
            EncoderParameters ep = new EncoderParameters(2);
            ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.MultiFrame);
            ep.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionLZW);
            bool firstPage = true;
            ImageCodecInfo info = GetEncoderInfo("image/tiff");
            foreach (System.Drawing.Image image in img)
            {
                if (firstPage)
                {
                    //pages = getBitonalImage(image);
                    pages = (image);
                    pages.Save(pathToSave, info, ep);
                    firstPage = false;
                }
                else
                {
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.FrameDimensionPage);
                    //System.Drawing.Image bmp = getBitonalImage(image);
                    System.Drawing.Image bmp = (image);
                    pages.SaveAdd(bmp, ep);
                    bmp.Dispose();
                    image.Dispose();
                }
            }

            //flush and close.
            ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.Flush);
            pages.SaveAdd(ep);
            pages.Dispose();
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }

            throw new Exception(mimeType + " mime type not found in ImageCodecInfo");
        }

        private static System.Drawing.Image getBitonalImage(System.Drawing.Image img)
        {
            if (img.PixelFormat == PixelFormat.Format1bppIndexed)
            {
                return img;
            }
            else
                return ConvertToBitonal((Bitmap)img);
        }

        //copied from http://www.codeproject.com/KB/GDI-plus/SaveMultipageTiff.aspx
        public static Bitmap ConvertToBitonal(Bitmap original)
        {
            Bitmap source = null;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert
            if (original.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(original, 0, 0);
                }
            }
            else
            {
                source = original;
            }

            // Lock source bitmap in memory
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy image data to binary array
            int imageSize = sourceData.Stride * sourceData.Height;
            byte[] sourceBuffer = new byte[imageSize];
            System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

            // Unlock source bitmap
            source.UnlockBits(sourceData);

            // Create destination bitmap
            Bitmap destination = new Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);

            // Lock destination bitmap in memory
            BitmapData destinationData = destination.LockBits(new Rectangle(0, 0, destination.Width, destination.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            // Create destination buffer
            imageSize = destinationData.Stride * destinationData.Height;
            byte[] destinationBuffer = new byte[imageSize];

            int sourceIndex = 0;
            int destinationIndex = 0;
            int pixelTotal = 0;
            byte destinationValue = 0;
            int pixelValue = 128;
            int height = source.Height;
            int width = source.Width;
            int threshold = 500;

            // Iterate lines
            for (int y = 0; y < height; y++)
            {
                sourceIndex = y * sourceData.Stride;
                destinationIndex = y * destinationData.Stride;
                destinationValue = 0;
                pixelValue = 128;

                // Iterate pixels
                for (int x = 0; x < width; x++)
                {
                    // Compute pixel brightness (i.e. total of Red, Green, and Blue values)
                    pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] + sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }
                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }
                    sourceIndex += 4;
                }
                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }

            // Copy binary image data to destination bitmap
            System.Runtime.InteropServices.Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

            // Unlock destination bitmap
            destination.UnlockBits(destinationData);

            // Return
            return destination;
        }


        public void generateThumbNails(System.Drawing.Image img, string dirToSave, int width)
        {

            if (img != null)
            {
                if (System.IO.Directory.Exists(dirToSave) != true)
                    System.IO.Directory.CreateDirectory(dirToSave);



                float ratio = (float)((float)img.Height / (float)img.Width);


                int height = (int)((float)width * ratio);

                int pages = totalPages(img);
                for (int i = 0; i < pages; i++)
                {
                    Bitmap bmp = convertTiffToJpeg(img, i, width, height);
                    String filePath = dirToSave + "\\" + (i + 1) + ".gif";

                    EncoderParameters ep = new EncoderParameters(2);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.RenderProgressive);
                    ep.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionLZW);

                    // bmp.Save(filePath, GetEncoderInfo("image/gif"), ep);
                    //  bmp.Dispose();


                    //help text for optimize graphic
                    //Image CloneImage(Image img)
                    //{
                    //Bitmap img2 = new Bitmap(img.Width, img.Height);
                    //using (Graphics g = Graphics.FromImage(img2);
                    //g.DrawImageUnscaled(img, 0, 0);
                    //return img2;
                    //}
                    //




                    Graphics oGraphic = default(Graphics);
                    // Here create a new bitmap object of the same height and width of the image.
                    Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height);
                    using (oGraphic = Graphics.FromImage(bmpNew))
                    {
                        oGraphic.DrawImage(bmp, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel);
                    }
                    // Release the lock on the image file. Of course,
                    // image from the image file is existing in Graphics object
                    bmp.Dispose();
                    bmp = bmpNew;

                    oGraphic.Dispose();
                    if (bmp.Height < 65)
                    {
                        bmp = new Bitmap(bmp, bmp.Width, bmp.Height + bmp.Height);
                    }
                    bmp.Save(filePath, ImageFormat.Gif);
                    bmp.Dispose();


                }
            }
            else
                throw new ArgumentException("Image file is not valid");
        }


        public static int totalPages(System.Drawing.Image img)
        {
            System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
            return img.GetFrameCount(dimention);
        }

        public static Bitmap convertTiffToJpeg(System.Drawing.Image img, int curPageNo, int width, int height)
        {
            System.Drawing.Imaging.FrameDimension dimention = new FrameDimension(img.FrameDimensionsList[0]);
            img.SelectActiveFrame(dimention, curPageNo);
            return (Bitmap)resizeImage(img, width, height);
        }

        public static System.Drawing.Image resizeImage(System.Drawing.Image img, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, 0, 0, width, height);
            g.Dispose();
            return bmp;
        }

        #region Tiff Handler
        public static string TiffHandler(string DirectoryPathWhereTiffIs, string fileName)
        {
            string returnValue = "";
            string TempPathForTiffToPdfFile = "";
            List<Bitmap> BitmapList = new List<Bitmap>();
            TiffEditor tiffEditor = new TiffEditor();
            try
            {
                BitmapList = tiffEditor.GetListOfImagesFromTiffFile(DirectoryPathWhereTiffIs);
                TempPathForTiffToPdfFile = DirectoryPathWhereTiffIs.Replace(fileName, "") + "pdf";
                if (!Directory.Exists(TempPathForTiffToPdfFile))
                {
                    Directory.CreateDirectory(TempPathForTiffToPdfFile);
                }
                TempPathForTiffToPdfFile += "\\" + "" + fileName.Replace(".tiff", "").Replace(".tif", "") + ".pdf";
                SaveBitmapImagesAsPdf(BitmapList, TempPathForTiffToPdfFile);
                returnValue = TempPathForTiffToPdfFile;
            }
            catch (Exception)
            {
                returnValue = "";
            }
            return returnValue;
        }
        public static string TiffHandlerForSavedDocuments(string DirectoryPathWhereTiffIs, string TempPathForTiffToPdfFile, string fileName)
        {
            string returnValue = "";
            List<Bitmap> BitmapList = new List<Bitmap>();
            TiffEditor tiffEditor = new TiffEditor();
            try
            {
                BitmapList = tiffEditor.GetListOfImagesFromTiffFile(DirectoryPathWhereTiffIs);
                if (!Directory.Exists(TempPathForTiffToPdfFile))
                {
                    Directory.CreateDirectory(TempPathForTiffToPdfFile);
                }
                TempPathForTiffToPdfFile += "\\" + "" + fileName.Replace(".tiff", "").Replace(".tif", "") + ".pdf";
                SaveBitmapImagesAsPdf(BitmapList, TempPathForTiffToPdfFile);
                returnValue = TempPathForTiffToPdfFile;
            }
            catch (Exception)
            {
                returnValue = "";
            }
            return returnValue;
        }
        private static void SaveBitmapImagesAsPdf(List<Bitmap> BitmapList, string PdfFilePath)
        {
            iTextSharp.text.Document tifDocument = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
            iTextSharp.text.pdf.PdfWriter tifWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(tifDocument, new System.IO.FileStream(PdfFilePath, System.IO.FileMode.Create));
            tifDocument.Open();
            iTextSharp.text.pdf.PdfContentByte cb = tifWriter.DirectContent;
            int count = BitmapList.Count;
            foreach (Bitmap bm in BitmapList)
            {
                using (System.Drawing.Bitmap bm1 = new System.Drawing.Bitmap(bm))
                {
                    tifDocument.SetPageSize(new iTextSharp.text.Rectangle(0, 0, bm1.Width, bm1.Height));
                    tifDocument.Open();
                    bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, 0);
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
                    // scale the image to fit in the page  
                    img.ScalePercent(60f / img.DpiX * 100);
                    img.ScaleAbsoluteHeight(bm.Height);
                    img.ScaleAbsoluteWidth(bm.Width);
                    img.SetAbsolutePosition(0, 0);
                    cb.AddImage(img);
                    tifDocument.NewPage();
                }
            }
            tifDocument.Close();
        }
        #endregion

    }
}
