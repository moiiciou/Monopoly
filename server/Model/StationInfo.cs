namespace server.Model
{
    public class StationInfo : CaseInfo
    {
        public bool isMortgaged { get; set; }
        public string Owner { get; set; }
        public int Price { get; set; }
        public int RentBase { get; set; }

        public string TextPrice { get; set; }
        public StationInfo(string text, int price, string skinPath, int angle, int posPlateau, int rent)
        {
            RentBase = rent;
            positionPlateau = posPlateau;
            ImageTemplate = skinPath;
            TextPrice = price.ToString() + " €";
            TextLabel = text;
            Rotation = angle;
            isMortgaged = false;
            Price = price;
            Owner = null;
        }
        public StationInfo()
        {
            RentBase = 0;
            Owner = null;
            Price = 0;
            isMortgaged = false;
        }
    }
}
