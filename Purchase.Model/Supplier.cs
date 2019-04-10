using System.ComponentModel.DataAnnotations;

namespace Purchase.Model
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(5)]
        public string Code { get; set; }

        public decimal Treshold { get; set; }
        [StringLength(50)]
        public string MainEmail { get; set; }



    }
}
