namespace BarcodeCreator.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BarcodeContentModel
    {
        [Required]
        public string Text { get; set; }
    }
}
