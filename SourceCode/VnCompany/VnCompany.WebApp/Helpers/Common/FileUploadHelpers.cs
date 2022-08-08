using VnCompany.WebApp.Settings;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace VnCompany.WebApp.Helpers
{
    public class FileUploadHelper
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(FileUploadHelper));
        public static char DirSeparator = System.IO.Path.DirectorySeparatorChar;

        private static readonly int exifOrientationID = 0x112; //274

        public static void ExifRotate(Image img)
        {
            if (!img.PropertyIdList.Contains(exifOrientationID))
                return;

            var prop = img.GetPropertyItem(exifOrientationID);
            int val = BitConverter.ToUInt16(prop.Value, 0);
            var rot = RotateFlipType.RotateNoneFlipNone;

            if (val == 3 || val == 4)
                rot = RotateFlipType.Rotate180FlipNone;
            else if (val == 5 || val == 6)
                rot = RotateFlipType.Rotate90FlipNone;
            else if (val == 7 || val == 8)
                rot = RotateFlipType.Rotate270FlipNone;

            if (val == 2 || val == 4 || val == 5 || val == 7)
                rot |= RotateFlipType.RotateNoneFlipX;

            if (rot != RotateFlipType.RotateNoneFlipNone)
                img.RotateFlip(rot);
        }

        //public static string UploadFile(HttpPostedFileBase file, string folderName)
        //{
        //    var folderPath = HttpContext.Current.Server.MapPath("~\\Content" + DirSeparator + "Uploads" + DirSeparator + folderName);
        //    // Check if we have a file
        //    if (null == file) return "";
        //    // Make sure the file has content
        //    if (!(file.ContentLength > 0)) return "";

        //    string fileName = DateTime.Now.Millisecond + file.FileName;
        //    string fileExt = Path.GetExtension(file.FileName);

        //    // Make sure we were able to determine a proper extension
        //    if (null == fileExt) return "";

        //    // Check if the directory we are saving to exists
        //    if (!Directory.Exists(folderPath))
        //    {
        //        // If it doesn't exist, create the directory
        //        Directory.CreateDirectory(folderPath);
        //    }

        //    // Set our full path for saving
        //    string path = folderPath + DirSeparator + fileName;

        //    // Save our file
        //    file.SaveAs(Path.GetFullPath(path));

        //    // Save our thumbnail as well
        //    ResizeImage(file, 70, 70, folderPath);

        //    // Return the filename
        //    return fileName;
        //}

        //public static string UploadPostedFile(HttpPostedFile file, string folderName, bool includeDatePath = false)
        //{
        //    if (includeDatePath)
        //        folderName = folderName + "/" + DateTime.Now.ToString("yyyy/MM/dd");

        //    var folderPath = HttpContext.Current.Server.MapPath("~/Media") + "/" + folderName;
        //    // Check if we have a file
        //    if (null == file) return "";
        //    // Make sure the file has content
        //    if (!(file.ContentLength > 0)) return "";

        //    var fileExt = Path.GetExtension(file.FileName);
        //    var fileName = TextHelpers.ConvertToUrlFriendly(Path.GetFileNameWithoutExtension(file.FileName), "img");

        //    var newFilePrefix = EpochTime.GetIntDate(DateTime.Now) + "_";

        //    var newFileName = newFilePrefix + fileName + fileExt;
        //    var absolutePath = folderName + "/" + newFileName;

        //    // Make sure we were able to determine a proper extension
        //    if (null == fileExt) return "";

        //    // Check if the directory we are saving to exists
        //    if (!Directory.Exists(folderPath))
        //    {
        //        // If it doesn't exist, create the directory
        //        //Directory.CreateDirectory(folderPath);
        //        EnsureFolder(folderPath);
        //    }

        //    // Set our full path for saving
        //    string path = folderPath + DirSeparator + newFileName;

        //    // Save our file
        //    try
        //    {
        //        var myImg = Image.FromStream(file.InputStream, true, true);
        //        ExifRotate(myImg);

        //        //Decrease size
        //        var newImgBytes = ResizeImage(myImg, myImg.Width, myImg.Height);
        //        var newImg = (Bitmap)((new ImageConverter()).ConvertFrom(newImgBytes));

        //        newImg.Save(Path.GetFullPath(path));
        //        //
        //        //var myImg = Image.FromStream(file.InputStream, true, true);
        //        //var newImgBytes = ResizeImage(myImg, myImg.Width, myImg.Height);
        //        //var newImg = (Bitmap)((new ImageConverter()).ConvertFrom(newImgBytes));

        //        //if (SystemSettings.WatermarkEnabled)
        //        //{
        //        //    AddWatermark((Image)newImg, SystemSettings.WatermarkPosition);
        //        //    //AddStringWatermark((Image)newImg, "Halo VN", ImageFormat.Jpeg);
        //        //}

        //        //newImg.Save(Path.GetFullPath(path));

        //        //if (img.Width > 1000)
        //        //    img.Resize(800, 600);

        //        //img.Save(Path.GetFullPath(path), null, false);
        //    }
        //    catch (Exception ex)
        //    {
        //        var strError = string.Format("Failed for UploadPostedFile: {0}", ex.ToString());

        //        _logger.Error(strError);

        //        return string.Empty;
        //    }

        //    // Return the filename
        //    return absolutePath;
        //}

        public static async Task<string> UploadImageAsync(IFormFile file, string folderName, bool includeDatePath = false)
        {
            if (includeDatePath)
                folderName = folderName + "/" + DateTime.Now.ToString("yyyy/MM/dd");

            //var folderPath = HttpContext.Current.Server.MapPath("~\\Media" + DirSeparator + folderName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + DirSeparator + "Media" + DirSeparator + folderName);

            // Check if we have a file
            if (null == file) return "";
            // Make sure the file has content
            if (!(file.Length > 0)) return "";

            var dtInt = EpochTime.GetIntDate(DateTime.Now);
            var generatedFileName = Utility.Md5HashingData(dtInt.ToString());

            var fileExt = Path.GetExtension(file.FileName);
            var fileName = TextHelpers.ConvertToUrlFriendly(Path.GetFileNameWithoutExtension(file.FileName), dtInt.ToString());

            var newFileName = generatedFileName + fileExt;
            var absolutePath = "/Media/" + folderName + "/" + newFileName;

            // Make sure we were able to determine a proper extension
            if (null == fileExt) return "";

            // Check if the directory we are saving to exists
            if (!Directory.Exists(folderPath))
            {
                // If it doesn't exist, create the directory
                Directory.CreateDirectory(folderPath);
            }

            // Set our full path for saving
            string path = folderPath + DirSeparator + newFileName;

            // Save our file
            try
            {
                var myImg = Image.FromStream(file.OpenReadStream());

                await Task.FromResult(myImg);

                ExifRotate(myImg);

                int maxImageWidth = 800;
                int newImageHeight = myImg.Height;

                if (myImg.Width > maxImageWidth)
                {
                    newImageHeight = (int)(myImg.Height * ((float)maxImageWidth / (float)myImg.Width));
                }
                else
                {
                    maxImageWidth = myImg.Width;
                }

                // Assumes myImage is the PNG you are converting
                using (var b = new Bitmap(maxImageWidth, newImageHeight))
                {
                    b.SetResolution(myImg.HorizontalResolution, myImg.VerticalResolution);

                    using (var g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.White);
                        //g.DrawImageUnscaled(myImg, 0, 0);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(myImg, 0, 0, maxImageWidth, newImageHeight);
                    }

                    // Now save b as a JPEG like you normally would
                    b.Save(Path.GetFullPath(path), ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed for UploadImage: {0}", ex.ToString());
                _logger.Error(strError);

                return string.Empty;
            }

            // Return the filename
            return absolutePath;
        }

        private static Image AddWatermark(Image image, string position = "top-left")
        {
            var x = 0;
            var y = 0;
            using (Image watermarkImage = Image.FromFile(CommonHelpers.MapPath("~/Content/images/watermark.png")))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                //int x = (image.Width / 2 - watermarkImage.Width / 2);
                //int y = (image.Height / 2 - watermarkImage.Height / 2);

                if (position == "top-left")
                {
                    x = watermarkImage.Width / 2; y = watermarkImage.Height / 2;
                }
                else if (position == "top-center")
                {
                    x = (image.Width / 2 - watermarkImage.Width / 2); y = watermarkImage.Height / 2;
                }
                else if (position == "top-right")
                {
                    x = (image.Width - (watermarkImage.Width + (watermarkImage.Width / 2))); y = watermarkImage.Height / 2;
                }
                else if (position == "middle-left")
                {
                    x = watermarkImage.Width / 2; y = (image.Height / 2 - watermarkImage.Height / 2);
                }
                else if (position == "middle-center")
                {
                    x = (image.Width / 2 - watermarkImage.Width / 2);
                    y = (image.Height / 2 - watermarkImage.Height / 2);
                }
                else if (position == "middle-right")
                {
                    x = (image.Width - (watermarkImage.Width + (watermarkImage.Width / 2))); y = (image.Height / 2 - watermarkImage.Height / 2);
                }
                else if (position == "bottom-left")
                {
                    x = watermarkImage.Width / 2; y = (image.Height - (watermarkImage.Height + (watermarkImage.Height / 2)));
                }
                else if (position == "bottom-center")
                {
                    x = (image.Width / 2 - watermarkImage.Width / 2); y = (image.Height - (watermarkImage.Height + (watermarkImage.Height / 2)));
                }
                else if (position == "bottom-right")
                {
                    x = (image.Width - (watermarkImage.Width + (watermarkImage.Width / 2)));
                    y = (image.Height - (watermarkImage.Height + (watermarkImage.Height / 2)));
                }
                else
                {
                    x = watermarkImage.Width / 2; y = watermarkImage.Height / 2;
                }

                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width, watermarkImage.Height)));
            }

            return image;
        }

        public static Image AddStringWatermark(Image image, string text, ImageFormat fmt)
        {

            try
            {
                // open source image as stream and create a memorystream for output
                Stream output = new MemoryStream();
                // choose font for text
                Font font = new Font("Arial", (int)((float)image.Width / 20), FontStyle.Bold, GraphicsUnit.Pixel);

                //choose color and transparency
                Color color = Color.FromArgb(100, 255, 0, 0);

                //location of the watermark text in the parent image
                Point pt = new Point(100, 50);
                SolidBrush brush = new SolidBrush(color);

                //draw text on image
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawString(text, font, brush, pt);
                graphics.Dispose();

                //update image memorystream
                //image.Save(output, fmt);
                // Image imgFinal = Image.FromStream(output);

                //write modified image to file
                //Bitmap bmp = new System.Drawing.Bitmap(image.Width, image.Height, image.PixelFormat);
                //Graphics graphics2 = Graphics.FromImage(bmp);
                //graphics2.DrawImage(imgFinal, new Point(0, 0));                

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("AddStringWatermark error because: {0}", ex.ToString()));
            }

            return image;
        }

        public static byte[] ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.Default;
                graphics.InterpolationMode = InterpolationMode.Default;
                graphics.SmoothingMode = SmoothingMode.Default;
                graphics.PixelOffsetMode = PixelOffsetMode.Default;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            MemoryStream ms = new MemoryStream();
            destImage.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }

        //public static string UploadVideo(HttpPostedFile file, string folderName, out string cover)
        //{
        //    cover = string.Empty;
        //    folderName = folderName + "/" + DateTime.Now.ToString("dd-MM-yyyy");
        //    var folderPath = SystemSettings.MediaFileUrl + "/" + folderName;
        //    // Check if we have a file
        //    if (null == file) return "";
        //    // Make sure the file has content
        //    if (!(file.ContentLength > 0)) return "";

        //    //string fileName = DateTime.Now.Millisecond + file.FileName;
        //    var fileExt = Path.GetExtension(file.FileName);
        //    var fileName = TextHelpers.ConvertToUrlFriendly(Path.GetFileNameWithoutExtension(file.FileName));
        //    var newFilePrefix = EpochTime.GetIntDate(DateTime.Now) + "_";
        //    var newFileName = newFilePrefix + fileName + fileExt;
        //    var absolutePath = folderName + "/" + newFileName;

        //    // Make sure we were able to determine a proper extension
        //    if (null == fileExt) return "";

        //    // Check if the directory we are saving to exists
        //    if (!Directory.Exists(folderPath))
        //    {
        //        // If it doesn't exist, create the directory
        //        Directory.CreateDirectory(folderPath);
        //    }

        //    // Set our full path for saving
        //    string path = folderPath + DirSeparator + newFileName;

        //    // Save our file
        //    try
        //    {
        //        var fullPath = Path.GetFullPath(path);
        //        file.SaveAs(fullPath);

        //        //Make sure the file was uploaded succesfully
        //        if (System.IO.File.Exists(fullPath))
        //        {
        //            //After upload success
        //            cover = folderName + "/" + fileName + "_thumb.png";
        //            var coverPath = folderPath + DirSeparator + newFilePrefix + fileName + "_thumb.png";
        //            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

        //            //Take the image after 5 seconds
        //            ffMpeg.GetVideoThumbnail(fullPath, coverPath, 5);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var strError = string.Format("Failed for UploadVideo: {0}", ex.ToString());
        //        logger.Error(strError);
        //    }

        //    // Return the filename
        //    return absolutePath;
        //}

        //public static string UploadPostedFileBase(HttpPostedFileBase file, string folderName)
        //{
        //    var folderPath = SystemSettings.MediaFileUrl + "/" + folderName;
        //    // Check if we have a file
        //    if (null == file) return "";
        //    // Make sure the file has content
        //    if (!(file.ContentLength > 0)) return "";

        //    string fileName = DateTime.Now.Millisecond + file.FileName;
        //    var absolutePath = folderName + "/" + file.FileName;
        //    string fileExt = Path.GetExtension(file.FileName);

        //    // Make sure we were able to determine a proper extension
        //    if (null == fileExt) return "";

        //    // Check if the directory we are saving to exists
        //    if (!Directory.Exists(folderPath))
        //    {
        //        // If it doesn't exist, create the directory
        //        Directory.CreateDirectory(folderPath);
        //    }

        //    // Set our full path for saving
        //    string path = folderPath + DirSeparator + fileName;

        //    // Save our file
        //    file.SaveAs(Path.GetFullPath(path));

        //    // Return the filename
        //    return absolutePath;
        //}

        public static void DeleteFile(string fileName, string folderPath)
        {
            // Don't do anything if there is no name
            if (fileName.Length == 0) return;

            // Set our full path for deleting
            string path = folderPath + DirSeparator + fileName;
            string thumbPath = folderPath + DirSeparator + "Thumbnails" + DirSeparator + fileName;

            RemoveFile(path);
            RemoveFile(thumbPath);
        }

        private static void RemoveFile(string path)
        {
            // Check if our file exists
            if (File.Exists(Path.GetFullPath(path)))
            {
                // Delete our file
                File.Delete(Path.GetFullPath(path));
            }
        }

        //public static void ResizeImage(HttpPostedFileBase file, int width, int height, string folderPath)
        //{
        //    string thumbnailDirectory = String.Format(@"{0}{1}", folderPath, DirSeparator);

        //    // Check if the directory we are saving to exists
        //    if (!Directory.Exists(thumbnailDirectory))
        //    {
        //        // If it doesn't exist, create the directory
        //        Directory.CreateDirectory(thumbnailDirectory);
        //    }

        //    // Final path we will save our thumbnail
        //    string imagePath = String.Format(@"{0}{1}{2}", thumbnailDirectory, DirSeparator, file.FileName);
        //    // Create a stream to save the file to when we're done resizing
        //    FileStream stream = new FileStream(Path.GetFullPath(imagePath), FileMode.OpenOrCreate);

        //    // Convert our uploaded file to an image
        //    Image OrigImage = Image.FromStream(file.InputStream);
        //    // Create a new bitmap with the size of our thumbnail
        //    Bitmap TempBitmap = new Bitmap(width, height);

        //    // Create a new image that contains are quality information
        //    Graphics NewImage = Graphics.FromImage(TempBitmap);
        //    NewImage.CompositingQuality = CompositingQuality.HighQuality;
        //    NewImage.SmoothingMode = SmoothingMode.HighQuality;
        //    NewImage.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //    // Create a rectangle and draw the image
        //    Rectangle imageRectangle = new Rectangle(0, 0, width, height);
        //    NewImage.DrawImage(OrigImage, imageRectangle);

        //    // Save the final file
        //    TempBitmap.Save(stream, OrigImage.RawFormat);

        //    // Clean up the resources
        //    NewImage.Dispose();
        //    TempBitmap.Dispose();
        //    OrigImage.Dispose();
        //    stream.Close();
        //    stream.Dispose();
        //}

        //public static string ResizeImageByScreenCroping(HttpPostedFileBase file, int screenWidth, string folderPath)
        //{
        //    var newFileName = string.Empty;
        //    newFileName = file.FileName;

        //    string thumbnailDirectory = String.Format(@"{0}", folderPath);

        //    // Check if the directory we are saving to exists
        //    if (!Directory.Exists(thumbnailDirectory))
        //    {
        //        // If it doesn't exist, create the directory
        //        Directory.CreateDirectory(thumbnailDirectory);
        //    }

        //    // Final path we will save our thumbnail
        //    //string imagePath = String.Format(@"{0}{1}", thumbnailDirectory, file.FileName);
        //    string imagePath = String.Format(@"{0}{1}", thumbnailDirectory, newFileName);
        //    imagePath = imagePath.Replace("//", "/");
        //    // Create a stream to save the file to when we're done resizing
        //    FileStream stream = new FileStream(Path.GetFullPath(imagePath), FileMode.OpenOrCreate);

        //    // Convert our uploaded file to an image
        //    Image OrigImage = Image.FromStream(file.InputStream);

        //    var ratio = OrigImage.Height / (double)OrigImage.Width;
        //    var newWidth = screenWidth;
        //    var newHeight = (int)(screenWidth * ratio);

        //    // Create a new bitmap with the size of our thumbnail
        //    Bitmap TempBitmap = new Bitmap(newWidth, newHeight);

        //    // Create a new image that contains are quality information
        //    Graphics NewImage = Graphics.FromImage(TempBitmap);
        //    NewImage.CompositingQuality = CompositingQuality.HighQuality;
        //    NewImage.SmoothingMode = SmoothingMode.HighQuality;
        //    NewImage.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //    // Create a rectangle and draw the image
        //    Rectangle imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
        //    NewImage.DrawImage(OrigImage, imageRectangle);

        //    // Save the final file
        //    TempBitmap.Save(stream, OrigImage.RawFormat);

        //    // Clean up the resources
        //    NewImage.Dispose();
        //    TempBitmap.Dispose();
        //    OrigImage.Dispose();
        //    stream.Close();
        //    stream.Dispose();

        //    return newFileName;
        //}

        /// <summary>
        /// Upload a file to a specific folder
        /// </summary>        
        //public static void UploadFileLocal(HttpPostedFileBase file, string strDestFolder)
        //{
        //    if (file == null || file.ContentLength == 0) return;
        //    string fileName = Path.GetFileName(file.FileName);
        //    string folderPath = EnsureFolder(strDestFolder);

        //    string filePath = folderPath + fileName;
        //    file.SaveAs(filePath);
        //}

        //public static string UploadFileFromByteArray(byte[] byteArray, string fileName, string strDestFolder)
        //{
        //    var returnPath = string.Empty;
        //    try
        //    {
        //        var realPath = HttpContext.Current.Server.MapPath("~\\CloudStorage" + DirSeparator + "EmailData\\Agencies" + DirSeparator + strDestFolder);
        //        //var realPath = string.Format("{0}/{1}", SystemSettings.EmailStorageFolder, strDestFolder);

        //        string folderPath = EnsureFolder(realPath);
        //        var filePath = folderPath + fileName;

        //        if (!File.Exists(filePath))
        //        {
        //            //Save to file
        //            File.WriteAllBytes(filePath, byteArray);
        //        }

        //        returnPath = string.Format("{0}/{1}/{2}", "CloudStorage/EmailData/Agencies", strDestFolder, fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(string.Format("UploadEmailAttachmentToFile error because: {0}", ex.ToString()));
        //        returnPath = string.Empty;
        //    }

        //    return returnPath;
        //}
   
        //public static string UploadEmailAttachmentToFile(HttpPostedFileBase file, string fileName, string strDestFolder)
        //{
        //    var returnPath = string.Empty;
        //    try
        //    {
        //        var realPath = HttpContext.Current.Server.MapPath("~\\CloudStorage" + DirSeparator + "EmailData\\Agencies" + DirSeparator + strDestFolder);
        //        //var realPath = string.Format("{0}/{1}", SystemSettings.EmailStorageFolder, strDestFolder);

        //        string folderPath = EnsureFolder(realPath);
        //        var filePath = folderPath + EpochTime.GetIntDate(DateTime.Now) + "_" + fileName;

        //        if (!File.Exists(filePath))
        //        {
        //            //Save to file
        //            file.SaveAs(filePath);
        //        }

        //        returnPath = string.Format("{0}/{1}/{2}", "CloudStorage/EmailData/Agencies", strDestFolder, fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("Could not UploadByteArrayToFile because: {0}", ex.ToString());
        //        returnPath = string.Empty;
        //    }

        //    return returnPath;
        //}

        /// <summary>
        /// Upload a file to a specific folder
        /// </summary>        
        public static void DeleteFile(string filePath)
        {
            try
            {
                var realPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                if (File.Exists(realPath))
                {
                    File.Delete(realPath);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not DeleteFile because: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Ensure a folder is correct and existed
        /// </summary>        
        public static string EnsureFolder(string strFolderPath)
        {
            if (string.IsNullOrEmpty(strFolderPath))
            {
                throw new ArgumentNullException("strFolderPath");
            }

            if (!strFolderPath.EndsWith("//"))
            {
                strFolderPath = strFolderPath + "//";
            }

            if (!Directory.Exists(strFolderPath))
            {
                Directory.CreateDirectory(strFolderPath);
            }

            return strFolderPath;
        }

        public static void MoveFileToFolder(string strFilePath, string strFolderPath)
        {
            try
            {
                string strFileName = Path.GetFileName(strFilePath);

                string strDestFolder = EnsureFolder(strFolderPath);
                string strDestFilePath = strDestFolder + strFileName;

                File.Move(strFilePath, strDestFilePath);
            }
            catch (Exception ex)
            {
                string strErrorMessage = ex.Message;
            }
        }

        public static void ClearFolder(string strFolderPath)
        {
            if (Directory.Exists(strFolderPath))
            {
                var dir = new DirectoryInfo(strFolderPath);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.IsReadOnly = false;
                    fi.Delete();
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    ClearFolder(di.FullName);
                    di.Delete();
                }
            }
        }

        public static string ToSize(Int64 value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (Int64)unit)).ToString("0.00");
        }

        public static IFormFile Base64ToImage(string base64Str, string fileName)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64Str);
                MemoryStream stream = new MemoryStream(bytes);

                IFormFile file = new FormFile(stream, 0, bytes.Length, fileName, fileName);

                return file;
            }
            catch (Exception ex)
            {
                _logger.Error("Base64ToImage error because: {0}", ex.ToString());
            }

            return null;
        }
    }

    public enum SizeUnits
    {
        Byte, KB, MB, GB, TB, PB, EB, ZB, YB
    }
}