using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace SalonWithRazor.Models
{
    public partial class EmployeePicture
    {
        [Display(Name = "Nuotrauka")]
        public byte[] PictureWithDefault
        {
            get
            {
                if (!Tools.Comparer.IsDefaultValue(Picture))
                {
                    return Picture;
                }
                else
                {
                    Image newImage = Image.FromFile("wwwroot\\User-default-icon.png");
                    ImageConverter _imageConverter = new ImageConverter();
                    return (byte[])_imageConverter.ConvertTo(newImage, typeof(byte[]));
                }
            }
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public byte[] Picture { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Employee Employee { get; set; }
    }
}
