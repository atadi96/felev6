using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;

namespace Hirportal.Persistence
{
    [Table("Images")]
    public class ArticleImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[ForeignKey("Article")]
        public Article Article { get; set; }
        public string FilePath { get; set; }

        [NotMapped]
        public string SmallPath =>
            Path.GetFileNameWithoutExtension(FilePath)
            + "_small"
            + Path.GetExtension(FilePath);

        [NotMapped]
        public string MediumPath =>
            Path.GetFileNameWithoutExtension(FilePath)
            + "_medium"
            + Path.GetExtension(FilePath);
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
