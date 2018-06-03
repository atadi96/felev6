using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Hirportal.Persistence;

namespace Hirportal.Persistence.DTO
{
    public class ImageDTO
    {
        public int Id { get; }

        public byte[] Data { get; set; }

        public string Hash
        {
            get
            {
                using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
                {
                    return Convert.ToBase64String(sha1.ComputeHash(Data));
                }
            }
        }

        public ImageDTO(byte[] data)
        {
            Id = -1;
            Data = data;
        }

        public ImageDTO(ArticleImage image)
        {
            Id = image.Id;
            Data = image.ImageData;
        }
    }
}
