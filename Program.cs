﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Baumprojekt
{
    class Baeume 
    {   //Anlegen der Baumelemente
        public double X_Koordinate {get; set;}
        public double Y_Koordinate {get; set;}
        public int ObjektID {get; set;}
        public int ID {get; set;}
        public string ?Objektschluessel {get; set;}
        public int Baumnummer {get; set;}
        public string ?Baumart {get; set;}
        public string ?BaumartZusatz {get; set;}
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
                Baumnummer = Convert.ToInt16(csvEntries[5]);
                Baumart = csvEntries[6];
                BaumartZusatz = csvEntries[7];
                Pflanzdatum = Convert.ToInt32(csvEntries[8]);
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
                    "\nPflanzdatum: " + Pflanzdatum;
        }
    }

    class Program
    {    
        //Sortiereung mit Quicksort  für Pflanzdatum
        static List<Baeume> quicksortPflanzdatum(List<Baeume> list)
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
                BaumListe = quicksortPflanzdatum(BaumListe);

                int bc2 = 1; //Nummer zur Kontrolle der Ausgabe
                foreach (Baeume aBaum in BaumListe)
                {
                    if (bc2>5){break;} //Abbrechen der Auflistung nach den ersten 5 Elementen
                    System.Console.WriteLine("______________________\nBaumdaten:");
                    System.Console.WriteLine("### {0} ###",bc2);
                    bc2 ++;
                    System.Console.WriteLine(aBaum.ToString());
                }
            }
        }
    }
}
