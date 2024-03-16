using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class FileUtility
    {
        public static string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName).ToLower();
        }

        public static bool IsValidFile(IFormFile file, double maxSizeMB = 1)
        {
            if (file == null) return false;
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".jpg" && extension != ".png" && extension != ".pdf" && extension != ".csv" && extension != ".doc" && extension != ".docx") return false;

            double megaBytes = maxSizeMB * 1024 * 1024;
            if (file.Length > megaBytes) return false;

            return true;
        }

        public static byte[] ConvertToByteArray(IFormFile file)
        {
            if (file == null) return null;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();
                return fileBytes;
            }
        }

        public static string CustomConvertToBase64(byte[] fileBytes = null, string contentType = "")
        {
            if (fileBytes == null || fileBytes.Length == 0) return "";
            var file = "";
            if (contentType == "JPEG" || contentType == "JPG" || contentType == "jpg")
            {
                file = "data:image/jpeg;base64," + Convert.ToBase64String(fileBytes);
            }
            else if (contentType == "PNG" || contentType == "png")
            {
                file = "data:image/png;base64," + Convert.ToBase64String(fileBytes);

            }

            else if (contentType == "doc" || contentType == "DOC")
            {
                file = "data:application/msword; base64," + Convert.ToBase64String(fileBytes);

            }

            else if (contentType == "docx" || contentType == "DOCX")
            {
                file = "data:application/vnd.openxmlformats;base64," + Convert.ToBase64String(fileBytes);

            }
            return file;
        }


        public static string ConvertToBase64(byte[] fileBytes = null)
        {
            if (fileBytes == null || fileBytes.Length == 0) return "";
            return Convert.ToBase64String(fileBytes);
        }

        public static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            string contentType = "";
            switch (extension)
            {
                case ".pdf":
                    {
                        contentType = "application/pdf";
                        break;
                    }
                case ".jpeg":
                case ".jpg":
                    {
                        contentType = "image/jpeg";
                        break;
                    }
                case ".png":
                    {
                        contentType = "image/png";
                        break;
                    }
                case ".zip":
                    {
                        contentType = "application/zip";
                        break;
                    }
                case ".rar":
                    {
                        contentType = "application/vnd.rar";
                        break;
                    }
                case ".doc":
                    {
                        contentType = "application/msword";
                        break;
                    }
                case ".docx":
                    {
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    }
                default:
                    break;
            }

            return contentType;

        }
    }
}
