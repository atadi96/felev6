using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace Hirportal.Persistence
{
    public enum ImageSize
    {
        Full,
        Medium,
        Small
    }

    [Table("Images")]
    public class ArticleImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[ForeignKey("Article")]
        [Required]
        public Article Article { get; set; }
        [Required]
        public byte[] ImageData { get; set; }

        [NotMapped]
        public object LinkData => new { id = Id, hash = ImageHash() };

        public byte[] SizedData(ImageSize requestSize = ImageSize.Full)
        {
            if (requestSize == ImageSize.Full)
            {
                return ImageData;
            }
            else
            {
                using (var originalMS = new MemoryStream(ImageData))
                {
                    Bitmap originalBM = new Bitmap(originalMS);
                    int width, height;
                    switch (requestSize)
                    {
                        case ImageSize.Medium:
                            width = 1000;
                            break;
                        case ImageSize.Small:
                            width = 500;
                            break;
                        default:
                            width = originalBM.Width;
                            break;
                    }
                    height = (int) ((float)width / originalBM.Width * originalBM.Height);
                    Image result = new Bitmap(originalBM, width, height);
                    var jpg = System.Drawing.Imaging.ImageFormat.Jpeg;
                    using (var outStream = new MemoryStream())
                    {
                        result.Save(outStream, jpg);
                        return outStream.ToArray();
                    }
                }
            }
        }

        public string ImageHash()
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(sha1.ComputeHash(ImageData));
            }
        }

        /*
        public static ArticleImage FromImage(string imgPath, Article article, Image data)
        {
            try
            {
                Image small = new Bitmap(data, new Size(500, (int)(data.Height * 500.0 / data.Width)));
                Image medium = new Bitmap(data, new Size(1000, (int)(data.Height * 1000.0 / data.Width)));
                string filename = Path.GetRandomFileName() + ".png";
                string filepath = Path.Combine(imgPath, filename);
                while (File.Exists(filepath))
                {
                    filename = Path.GetRandomFileName() + ".png";
                    filepath = Path.Combine(imgPath, filename);
                }
                var png = System.Drawing.Imaging.ImageFormat.Png;
                ArticleImage artImage = new ArticleImage() { FilePath = filename };
                data.Save(filepath, png);
                small.Save(Path.Combine(imgPath, artImage.SmallPath));
                medium.Save(Path.Combine(imgPath, artImage.MediumPath));
                return artImage;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }*/
    }
}
