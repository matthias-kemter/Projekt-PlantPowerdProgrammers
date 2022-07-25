using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Baumprojekt
{
    class Baeume 
    {   //Anlegen der Baumelemente (Properties)
        public double X_Koordinate {get; set;}
        public double Y_Koordinate {get; set;}
        public int ObjektID {get; set;}
        public int ID {get; set;}
        public string ?Objektschluessel {get; set;}
        public string ?Baumnummer {get; set;}
        public string ?BaumartLatein {get; set;}
        public string ?BaumartDeutsch {get; set;}
        public int Pflanzdatum {get; set;}

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
                Baumnummer = csvEntries[5];
                BaumartLatein = csvEntries[6];
                if (BaumartLatein == null)
                {
                    BaumartLatein = "Ignotus";
                }
                BaumartDeutsch = csvEntries[7];
                if (BaumartDeutsch == null)
                {
                    BaumartDeutsch = "Unbekannt";
                }
                if (csvEntries[8].Length == 0) //Korrektur für nicht erkanntes Pflanzdatum zu 999 um Fehler beim sortieren zu vermeiden
                {
                    Pflanzdatum = 9999;
                }
                else{
                    Pflanzdatum = Convert.ToInt32(csvEntries[8]);
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
                    "\nBaumart: " + BaumartLatein + ", " + BaumartDeutsch +
                    "\nPflanzdatum: " + Pflanzdatum;
        }
    }

    class Umkreis{
         List<Baeume> AlteBaumListe = new List<Baeume>();


        //50 ältesten Bäume
        //sortieren nach südlichsten,nördlichste,westl. und östlichen
        //nord/süd und ost/west Durchschnitt
        //--> neue koordinaten für mittelpunkt
        //Abstand zu allen 50 bäumen
        //--> länsgter abstsnd ist umkreis um Mittelpunkt mit den ältesten bäumen

    }
    class Sortieren
    {
        //Sortiereung mit Quicksort  für Pflanzdatum
        public static List<Baeume> quicksortPflanzdatum(List<Baeume> list)
        {
            if (list.Count <= 1) return list;
            int pivotPosition = list.Count / 2;
            int pivotValue = list[pivotPosition].Pflanzdatum;
            Baeume pivot = list[pivotPosition];
            list.RemoveAt(pivotPosition);
            List<Baeume> smaller = new List<Baeume>();
            List<Baeume> greater = new List<Baeume>();
            foreach (Baeume item in list)
            {
                if (item.Pflanzdatum < pivotValue)
                {
                    smaller.Add(item);
                }
                else
                {
                    greater.Add(item);
                }
            }
            List<Baeume> sorted = quicksortPflanzdatum(smaller); 
            sorted.Add(pivot);
            sorted.AddRange(quicksortPflanzdatum(greater));
            return sorted;
        }
    }
    class Program
    {    
        
        static void Main(string[] args)
        {
            // Dateiname von csv
            string pathBaeumeCsv = @"./csv/baeume.csv"; //./csv/baeume.csv
            //Anzahl der Bäume in Csv
            int anzahlInListe = 49886;

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
                    for (int i = 1; i <= anzahlInListe; i++)

                    {
                        BaumListe.Add(new Baeume(baeumeAsCsvString[i]));
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    System.Console.WriteLine("Out Of Range");
                }

                int bc1 = 1; //Nummer zur Kontrolle der Ausgabe
                // Daten ausgeben lassen mit ToString()
                foreach (Baeume aBaum in BaumListe)
                {
                    System.Console.WriteLine("______________________\nBaumdaten:");
                    System.Console.WriteLine("### {0} ###",bc1);
                    bc1 ++;
                    System.Console.WriteLine(aBaum.ToString());
                }
                
                System.Console.WriteLine("\n #=#=#=#=#=#=#=#=#=#=#=#=#=#=#=NOW SORTED=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#\n");
                
                //Aufruf sort-function
                BaumListe = Sortieren.quicksortPflanzdatum(BaumListe);

                int bc2 = 1; //Nummer zur Kontrolle der Ausgabe
                foreach (Baeume aBaum in BaumListe)
                {
                    if (bc2>50){break;} //Abbrechen der Auflistung nach den ersten 500 Elementen
                    System.Console.WriteLine("______________________\nBaumdaten:");
                    System.Console.WriteLine("### {0} ###",bc2);
                    bc2 ++;
                    System.Console.WriteLine(aBaum.ToString());
                }
            }
        }
    }
}
