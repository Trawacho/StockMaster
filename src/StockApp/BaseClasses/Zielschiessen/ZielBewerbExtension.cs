using System.Linq;

namespace StockApp.BaseClasses.Zielschiessen
{
    internal static class ZielBewerbExtension
    {
        

        public static Turnier GetSampleZielBewerbTurnier()
        {
            Turnier t = new Turnier
            {
                OrgaDaten = new OrgaDaten()
            };

            var bewerb = new Zielbewerb();
            bewerb.AddNewTeilnehmer();
            bewerb.AddNewTeilnehmer();
            bewerb.AddNewTeilnehmer();
            bewerb.AddNewTeilnehmer();
            bewerb.AddNewTeilnehmer();

            var t1 = bewerb.Teilnehmerliste.First(t => t.Startnummer == 1);
            t1.FirstName = "Hans";
            t1.LastName = "Dampf";
            t1.Vereinsname = "ESF Hankofen";
            t1.LicenseNumber = "02/85859";
            t1.AddNewWertung();
            t1.AddNewWertung();
            t1.AddNewWertung();
            t1.AddNewWertung();

            t1.SetAktuelleBahn(1, 1);

            t.Wettbewerb = bewerb;

            return t;
        }
       
    }
}
