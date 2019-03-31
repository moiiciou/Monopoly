namespace server
{
    public class CardInfo
    {
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
        public int value2;

        public CardInfo(string label, string text, TypeCard typeCard, TypeAction typeEffet, int value)
        {
            this.Label = label;
            this.Text = text;
            this.value = value;
            this.typeCard = typeCard;
            this.typeAction = typeEffet;


        }
        public CardInfo(string label, string text, TypeCard typeCard, TypeAction typeEffet, int costMaison, int costHostel)
        {
            this.Label = label;
            this.Text = text;
            this.value = costMaison;
            value2 = costHostel;
            this.typeCard = typeCard;
            this.typeAction = typeEffet;


        }

        public CardInfo()
        {

        }
    }
}