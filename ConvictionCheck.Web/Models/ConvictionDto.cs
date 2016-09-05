using System.ComponentModel.DataAnnotations;
using ConvictionCheck.Web.Attributes;

namespace ConvictionCheck.Web.Models
{
    public class ConvictionDto
    {
        [Required(ErrorMessage = "Необходимо указать")]
        [Display(Name = "Есть судимость")]
        public bool? IsHaveConviction { get; set; }

        [RequiredIf("IsHaveConviction", ErrorMessage = "Если есть судимость, необходимо заполнить")]
        [Display(Name = "Номер статьи")]
        public string ArticleNumber { get; set; }
    }
}