using System.ComponentModel.DataAnnotations;

namespace BarcodeCreator.Models
{
    public class BarcodeContentModel
    {
        [Required]
        public string Text { get; set; }
    }
}
