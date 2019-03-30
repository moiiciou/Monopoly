namespace server
{
    public class CardInfo
    {
        /*
        public string TextPropertyName { get; set; }
        public string TextHouseCost { get; set; }
        public string TextHotelCost { get; set; }

        public string TextRentWith1House { get; set; }
        public string TextRentWith2House { get; set; }
        public string TextRentWith3House { get; set; }
        public string TextRentWith4House { get; set; }
        public string TextRentWithHotel { get; set; }

        public int RentValue { get; set; }
        public int HouseCost { get; set; }
        public int HostelCost { get; set; }


        public int RentWith1House { get; set; }
        public int RentWith2House { get; set; }
        public int RentWith3House { get; set; }
        public int RentWith4House { get; set; }
        public int RentWithHotel { get; set; }

        public string TextRentValue { get; set; }

        public string TextMortgageValue { get; set; }


        public string Color { get; set; }
        */
        public string Label { get; set; }
        public string Text { get; set; }
        public string Effect { get; set; }

        public enum TypeAction
        {
            reparation = 1,
            paiement = 2,
            moove = 3,
            freefromjail = 4
        };

        public enum TypeCard
        {
            community = 1,
            chance = 2,

        };

        public TypeAction typeAction;
        public TypeCard typeCard;

        public int value;

        public CardInfo(string label, string text, TypeCard typeCard, TypeAction typeEffet, int value)
        {
            this.Label = label;
            this.Text = text;
            this.value = value;
            this.typeCard = typeCard;
            this.typeAction = typeEffet;


        }

        public CardInfo()
        {

        }
    }
}