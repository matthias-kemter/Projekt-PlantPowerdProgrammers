using System;
using System.Collections;


namespace Baumprojekt
{
    class Baeume 
    {
        public double X_Koordinate {get; set;}
        public double Y_Koordinate {get; set;}
        public int ObjektID {get; set;}
        public int ID {get; set;}
        public string ?Objektschluessel {get; set;}
        public int Baumnummer {get; set;}
        public string ?Baumart {get; set;}
        public int Pflanzdatum {get; set;}

        public Baeume() { }
        public Baeume(string csvString)
        {
            string [] csvEntries = csvString.Split(',');
            
            // LEFT TO CHECK: Convert ToInt16/32/64
            X_Koordinate = Convert.ToDouble(csvEntries[0]);
            Y_Koordinate = Convert.ToDouble(csvEntries[1]);
            ObjektID = Int32.Parse(csvEntries[2]);
            ID = Int32.Parse(csvEntries[3]);
            Objektschluessel = csvEntries[4];
            Baumnummer = Int32.Parse(csvEntries[5]);
            Baumart = csvEntries[6] + "," + csvEntries[7];
            Pflanzdatum = Int32.Parse(csvEntries[8]);
        }

        public override string ToString()
        {
            return  "X-Koordinate: " + X_Koordinate + 
                    "\nY-Koordinate: " + Y_Koordinate + 
                    "\nObjektID: " + ObjektID +
                    "\nID: " + ID +
                    "\nObjektschlüssel: " + Objektschluessel +
                    "\nBaumnummer: " + Baumnummer +
                    "\nBaumart: " + Baumart + 
                    "\nPflanzdatum: " + Pflanzdatum;
        }
    }
    class Program
    {    static void Main(string[] args)
        {
            // Dateiname von csv
            string pathBaeumeCsv = @"baeume_kurz100.csv";

            // Liste von Bäumen erstellen
            List<Baeume> BaumListe = new List<Baeume>();

            // Liste von Bäumen mit csv Einträgen füllen
            if(File.Exists(pathBaeumeCsv))
            {
                // Zeile lesen
                string[] baeumeAsCsvString = File.ReadAllLines(pathBaeumeCsv);

                // Daten in Liste schreiben
                for (int i = 1; i <= 38; i++)
                {
                    BaumListe.Add(new Baeume(baeumeAsCsvString[i]));
                }

                // Daten ausgeben lassen
                foreach (Baeume aBaum in BaumListe)
                {
                    System.Console.WriteLine("______________________\nBaumdaten:\n");
                    System.Console.WriteLine(aBaum.ToString());
                }
            }
        }
    }
}