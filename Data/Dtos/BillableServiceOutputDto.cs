namespace Marigold
{
    public class BillableServiceOutputDto
    {
        public string Name { get; set; }

        public int UnitPrice { get; set; }

        public int Units { get; set; }

        public int Total { get { return Units * UnitPrice; } }

        public string UnitDescription { get; set; }
    }
}