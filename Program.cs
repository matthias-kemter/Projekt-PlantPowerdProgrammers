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
        public string ?BaumartZusatz {get; set;}
        public string ?Pflanzdatum {get; set;}
        public int pflanzdatumInt {get; set;}

        public Baeume() { }
        public Baeume(string csvString)
        {
            // In den csvEntries Array werden die jeweiligen Daten aus der einzulesenden csv Datei (csvString) als strings gespeichert
            string [] csvEntries = csvString.Split(',');
            
            // Der Array wird von 0 ausgehend abgelaufen und die Werte werden in der jeweiligen Reihenfolge (bzgl. der Reihenfolge der Kategorien im Tabellenkopf aka Index 0 der beim Einlesen der Daten (siehe // Daten in Liste schreiben) in der Main Funktion übersprungen wird) gespeichert
            try
            {
                X_Koordinate = Convert.ToDouble(csvEntries[0]);
                Y_Koordinate = Convert.ToDouble(csvEntries[1]);
                ObjektID = Convert.ToInt16(csvEntries[2]);
                ID = Convert.ToInt32(csvEntries[3]);
                Objektschluessel = csvEntries[4];
                Baumnummer = Convert.ToInt16(csvEntries[5]);
                Baumart = csvEntries[6];
                BaumartZusatz = csvEntries[7];
                // Hier kann man zwar noch Pflanzdatum = Convert.ToInt32(csvEntries[8]; schreiben, aber man bekommt auch so nur "0" als Ausgabe)
                Pflanzdatum = csvEntries[8];
                try
                {
                    int pflanzdatumInt = Convert.ToInt32(Pflanzdatum);
                }
                catch (FormatException)
                {
                    // Warum bekomme ich hier eine Bad Format Exception wenn ich einen String z.B."2009" zu einem Int32 umwandeln möchte?
                    System.Console.WriteLine("Bad Format");
                }
                catch (OverflowException)
                {
                    // Selbe Frage hier: Warum Overflow?
                    System.Console.WriteLine("Overflow");
                }
            }
            catch (FormatException)
            {
                System.Console.WriteLine("Bad Format");
            }
            catch (OverflowException)
            {
                System.Console.WriteLine("Overflow");
            }
        }
        
        // ToString() wird für eine übersichtliche Ausgabe überschrieben  
        public override string ToString()
        {
            // Formatierung der Konsolenausgabe als Liste
            return  "X-Koordinate: " + X_Koordinate + 
                    "\nY-Koordinate: " + Y_Koordinate + 
                    "\nObjektID: " + ObjektID +
                    "\nID: " + ID +
                    "\nObjektschlüssel: " + Objektschluessel +
                    "\nBaumnummer: " + Baumnummer +
                    "\nBaumart: " + Baumart + ", " + BaumartZusatz +
                    "\nPflanzdatumInt: " + pflanzdatumInt +
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

            
            // BaumListe mit Einträgen aus der csv füllen
            if(File.Exists(pathBaeumeCsv))
            {
                // Zeile lesen
                string[] baeumeAsCsvString = File.ReadAllLines(pathBaeumeCsv);

                // Daten in Liste schreiben
                try
                {
                    for (int i = 1; i <= 101; i++)
                    {
                        BaumListe.Add(new Baeume(baeumeAsCsvString[i]));
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    System.Console.WriteLine("Out Of Range");
                }

                // Daten ausgeben lassen mit ToString()
                foreach (Baeume aBaum in BaumListe)
                {
                    System.Console.WriteLine("______________________\nBaumdaten:\n");
                    System.Console.WriteLine(aBaum.ToString());
                }
            }
        }
    }
}
