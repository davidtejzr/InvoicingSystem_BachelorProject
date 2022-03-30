namespace TEJ0017_FakturacniSystem
{
    public class NumericalSeriesGenerator
    {
        private Models.Subject.OurCompany ourCompany;
        private int NextNum;
        public NumericalSeriesGenerator()
        {
            ourCompany = Models.Subject.OurCompany.getInstance();
        }

        public string generateDocumentNumber()
        {
            System.DateTime dateTime = DateTime.Now;

            //generovani poradi ciselne rady - s prichodem noveho roku se vytvori novy klic, ktery bude opet cislovany od 1
            int nextNum = 0;
            if (ourCompany.NumSeries.ContainsKey(dateTime.Year.ToString()))
            {
                nextNum = int.Parse(ourCompany.NumSeries[dateTime.Year.ToString()]);
                nextNum++;
            }
            else
            {
                nextNum = 1;
            }

            //doplneni znakem '0' aby celkova delka odpovidala nastaveni uzivatele
            return dateTime.Year.ToString() + nextNum.ToString().PadLeft((int)ourCompany.DocumentNumberLength, '0');
        }

        //aplikace nactene ciselne rady (pokud dojde k rucnimu prepisu, tak nove cislo bude poslednim v rade bez ohledu na predchozim postupu)
        public void saveChanges()
        {
            System.DateTime dateTime = DateTime.Now;

            int nextNum = 0;
            if (ourCompany.NumSeries.ContainsKey(dateTime.Year.ToString()))
            {
                nextNum = int.Parse(ourCompany.NumSeries[dateTime.Year.ToString()]);
                nextNum++;
                ourCompany.NumSeries[dateTime.Year.ToString()] = nextNum.ToString();
            }
            else
            {
                nextNum = 1;
                ourCompany.NumSeries.Add(dateTime.Year.ToString(), "1");
            }
        }
    }
}
