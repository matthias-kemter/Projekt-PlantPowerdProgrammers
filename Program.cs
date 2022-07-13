using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Baumprojekt
{
    class Baeume 
    {
        public float X_Koordinate {get; set;}
        public float Y_Koordinate {get; set;}
        public int ObjektID {get; set;}
        public int ID {get; set;}
        public int Objektschluessel {get; set;}
        public int Baumnummer {get; set;}
        public string ?Baumart {get; set;}
        public int Pflanzdatum {get; set;}

        public Baeume() { }
        public Baeume(string csvString)
        {
            string [] csvEntries = csvString.Split(',');
            
            // LEFT TO CHECK: Convert ToInt16/32/64
            X_Koordinate = float.Parse(csvEntries[0]);
            Y_Koordinate = float.Parse(csvEntries[1]);
            ObjektID = Convert.ToInt16(csvEntries[2]);
            ID = Convert.ToInt16(csvEntries[3]);
            Objektschluessel = Convert.ToInt16(csvEntries[4]);
            Baumnummer = Convert.ToInt16(csvEntries[5]);
            Baumart = csvEntries[6];
            Pflanzdatum = Convert.ToInt16(csvEntries[7]);
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
            string pathBaeumeCsv = @"Neu.csv";

            // Liste von Bäumen erstellen
            List<Baeume> BaumListe = new List<Baeume>();

            // Liste von Bäumen mit csv Einträgen füllen
            if(File.Exists(pathBaeumeCsv))
            {
                // Zeile lesen
                string[] baeumeAsCsvString = File.ReadAllLines(pathBaeumeCsv);

                // Daten in Liste schreiben
                BaumListe.Add(new Baeume(baeumeAsCsvString[0]));

                // Daten ausgeben lassen
                foreach (Baeume aBaum in BaumListe)
                {
                    System.Console.WriteLine(aBaum.ToString());
                    System.Console.WriteLine("Hello World!");
                }
            }
            //Sortieren
            BaumListe.Sort
            (
                delegate(Baeume x, Baeume y) 
                {
                    return x.Pflanzdatum.CompareTo(y.Pflanzdatum);
                }
            );
        }
    }
}