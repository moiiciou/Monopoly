namespace server
{
    public class CaseInfo
    {
        public string ImageTemplate { get; set; }
        public string Location { get; set; }
        public int Price { get; set; }
        public int Rent { get; set; }
        public int NumberOfHouse { get; set; } = 0;

        public int HouseCost { get; set; }
        public int HostelCost { get; set; }

        public bool HasHostel { get; set; } = false;
        public string Color { get; set; }
        public int Rotation { get; set; }
        public string Owner { get; set; }
        public string TxtPrice { get; set; }

    }
}