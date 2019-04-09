namespace Purchase.Model
{
    public class Supplier
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(50)]
        public string Name { get; set; }

        public string Code { get; set; }

        public decimal Treshold { get; set; }

        public string MainEmail { get; set; }



    }
}
