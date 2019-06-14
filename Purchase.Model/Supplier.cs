using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Purchase.Model
{
    public class Supplier
    {
        public Supplier()
        {
            PhoneNumbers = new Collection<SupplierPhoneNumber>();
            Meetings = new Collection<Meeting>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(5)]
        public string Code { get; set; }

        public decimal Treshold { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string MainEmail { get; set; }

        public int? TypeOfSupplierId { get; set; }

        public SupplierType TypeOfSupplier { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<SupplierPhoneNumber> PhoneNumbers { get; set; }

        public ICollection<Meeting> Meetings { get; set; }

    }
}
