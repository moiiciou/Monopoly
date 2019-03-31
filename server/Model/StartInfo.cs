namespace server.Model
{
    public class StartInfo : CaseInfo
    {
        public int Income;
        public StartInfo(string text, int income, string skinPath, int posPlateau)
        {
            positionPlateau = posPlateau;


            Income = income;
            TextLabel = text;
            ImageTemplate = skinPath;


        }
        
    }

    public class JailInfo : CaseInfo
    {
        public int priceOfFreedom;
        public JailInfo(string text, string skinPath, int posPlateau)
        {
            positionPlateau = posPlateau;
            priceOfFreedom = 50;
            TextLabel = text;
            ImageTemplate = skinPath;


        }

    }
}
