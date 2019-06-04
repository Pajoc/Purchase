using System.ComponentModel.DataAnnotations;

namespace Purchase.Model
{
    public class SupplierType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

    }
}
