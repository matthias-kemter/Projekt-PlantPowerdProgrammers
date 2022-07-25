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
        public int Baumnummer {get; set;}
        public string ?BaumartLatein {get; set;}
        public string ?BaumartDeutsch {get; set;}
        public int Pflanzdatum {get; set;}
        public Baeume() { }
        public Baeume(string csvString)
        {
            // In den csvEntries Array werden die jeweiligen Daten aus der einzulesenden csv Datei (csvString) als strings gespeichert
            string [] csvEntries = csvString.Split(',');
            
            // Der Array wird von 0 ausgehend abgelaufen und die Werte werden in der jeweiligen Reihenfolge (bzgl. der Reihenfolge der Kategorien 
            //im Tabellenkopf aka Index 0 der beim Einlesen der Daten (siehe // Daten in Liste schreiben) in der Main Funktion übersprungen wird) gespeichert
            try
            {   
                X_Koordinate = Convert.ToDouble(csvEntries[0]);
                Y_Koordinate = Convert.ToDouble(csvEntries[1]);
                ObjektID = Convert.ToInt16(csvEntries[2]);
                ID = Convert.ToInt32(csvEntries[3]);
                Objektschluessel = csvEntries[4];
                Baumnummer = Convert.ToInt32(csvEntries[5]);
                BaumartLatein = csvEntries[6];
                if (BaumartLatein == null) //Korrektur für nicht erkannte Baumart bzw. nicht in Csv angegeben
                {
                    BaumartLatein = "Ignotus";
                }
                BaumartDeutsch = csvEntries[7];
                if (BaumartDeutsch == null)
                {
                    BaumartDeutsch = "Unbekannt";
                }
                if (csvEntries[8].Length == 0) //Korrektur für nicht erkanntes Pflanzdatum zu 9999 um Fehler beim sortieren zu vermeiden
                {
                    Pflanzdatum = 9999;
                }
                else{
                    Pflanzdatum = Convert.ToInt32(csvEntries[8]);
                }
            }
            catch (FormatException) //Exception-Handling
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
    
    class Sortieren
    {
        //Sortiereung mit Quicksort  für Pflanzdatum
        public static List<Baeume> quicksortPflanzdatum(List<Baeume> list)
        {
            if (list.Count <= 1) return list;
            int pivotPosition = list.Count / 2; //Auswahl Pivot-Element
            int pivotValue = list[pivotPosition].Pflanzdatum;
            Baeume pivot = list[pivotPosition];
            list.RemoveAt(pivotPosition);
            //Vergleich ob größer oder kleiner
            List<Baeume> smaller = new List<Baeume>();
            List<Baeume> greater = new List<Baeume>();
            foreach (Baeume item in list) //Baum Positionierung auf sortierter Liste
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
            //Sortierte Baumliste
            List<Baeume> sorted = quicksortPflanzdatum(smaller); 
            sorted.Add(pivot);
            sorted.AddRange(quicksortPflanzdatum(greater));
            return sorted;
        }
    }

    class Umkreis : Sortieren
    { 
        //Variabeln für die Mittelpunktberechnung
        public static double midwe {get; set;}//Mitte westlichste und östlichste Koordinate
        public static double midns {get; set;}//Mitte nördlichste und südlichste Koordinate
        //extrembäume in der himmelsrichtungen
        public static double nordtree {get; set;} 
        public static double southtree {get; set;}
        public static double westtree {get; set;}
        public static double easttree {get; set;}
        public static double abstand {get; set;} //Abstand von Mittelpunkt zum Entferntesten Baum
        public static double [] Wests = new double [49]; //Westkoordinaten
        public static double [] Nords = new double [49]; //Nordkoordinaten
        public void Rechnen()
        { 
            //sortieren nach x-Koordinate
            static List<Baeume> quicksortNord(List<Baeume> list) 
            {
                if (list.Count <= 1) return list;
                int pivotPosition = list.Count / 2; //Auswahl Pivot-Element
                double pivotValue = list[pivotPosition].Y_Koordinate; 
                Baeume pivot = list[pivotPosition];
                list.RemoveAt(pivotPosition); 
                //Vergleich ob größer oder kleiner
                List<Baeume> smaller = new List<Baeume>();
                List<Baeume> greater = new List<Baeume>();
                foreach (Baeume item in list)
                {
                    if (item.Y_Koordinate < pivotValue)
                    {
                        smaller.Add(item);
                    }
                    else
                    {
                        greater.Add(item);
                    }
                }
                List<Baeume> sorted = quicksortNord(smaller); 
                sorted.Add(pivot);
                sorted.AddRange(quicksortNord(greater));
                
                //Bestimmen des nördlichsten und südlichsten Baums
                nordtree = list[0].Y_Koordinate;
                southtree = list[49].Y_Koordinate;
                midns =  (nordtree + southtree)/2;

                for(int i = 0; i >= 49 ;i++){ //Array sortiert nach Norden
                    Nords [i]= list[0].X_Koordinate;
                }
                
                return sorted;
            }

            //sortieren nach x-Koordinate
            static List<Baeume> quicksortWest(List<Baeume> list)
            {
                if (list.Count <= 1) return list;
                int pivotPosition = list.Count / 2; //Auswahl Pivot-Element
                double pivotValue = list[pivotPosition].X_Koordinate;
                Baeume pivot = list[pivotPosition];
                list.RemoveAt(pivotPosition);
                //Vergleich ob größer oder kleiner
                List<Baeume> smaller = new List<Baeume>();
                List<Baeume> greater = new List<Baeume>();
                foreach (Baeume item in list) //Baum Positionierung auf sortierter Liste
                {
                    if (item.X_Koordinate < pivotValue)
                    {
                        smaller.Add(item);
                    }
                    else
                    {
                        greater.Add(item);
                    }
                }
                List<Baeume> sorted = quicksortWest(smaller); 
                sorted.Add(pivot);
                sorted.AddRange(quicksortNord(greater));
                
                //Bestimmen des westlichseten und östlichsten Baums
                westtree = list[0].X_Koordinate;
                easttree = list[49].X_Koordinate;
                midwe =  (westtree + easttree)/2;

                for(int i = 0; i >= 49 ;i++){ //Array sortiert nach Westen
                    Wests [i]= list[0].X_Koordinate;
                }
                
                return sorted;
            }

            //Abstandsberechnug mit Hilfe des Satz von Pythagoras
            abstand =  0.0;
            for(int i = 0; i >= 49; i++){
                double testabstand = Math.Sqrt(Math.Pow((midwe-Wests[i]), 2)+Math.Pow((midns-Nords[i]),2));
                //wurzel((b1−a1)^2+(b2−a2)^2)   //a mittelpunkt, b gesuchter punkt
                if(testabstand > abstand){
                    abstand = testabstand;
                };
            }
            //nord/süd und ost/west Durchschnitt    //--> neue koordinaten für mittelpunkt
            //Abstand zu allen 50 bäumen    //--> länsgter abstsnd ist umkreis um Mittelpunkt mit den ältesten bäumen
        }
        public void OutputCheck()
        {       //Testen und überprüfen der Ausgabe
                System.Console.WriteLine(westtree);
                System.Console.WriteLine(easttree);
                System.Console.WriteLine(abstand);
        }
    }

    class Testen
    {
        public bool testSuccess {get; set;}
        // Testfunktion für den Abstand - falls er ungleich 0 ist, wird er richtig ausgerechnet. Zur Zeit ist ist das Ergebnis immer 0
        public void CheckAbstand (double abstand)
        {
            if (abstand != 0)
            {
                System.Console.WriteLine("Abstand-Test bestanden!"); //Ergebnis wenn wir was können
            }else
            {
                System.Console.WriteLine("Abstand-Test fehlgeschlagen!"); //Ergebnis wenn der Computer, nicht das macht was wir wollen
            }
        }
        // Testen ob Sortieren erfolgreich
        public void CheckIfAscending (List<Baeume> list)
        {
            // Es reicht hier nur die ersten 50 Einträge zu checken, da diese hinreichend sind um eine Aussage über die Funktionalität des Sortieralgorithmus zu treffen
            for (int i = 0; i < 49; i++)
            {
                if (list[i].Pflanzdatum <= list[i+1].Pflanzdatum){ //Kontrolle ob Pflanzdatum aufsteigend sortiert ist
                    testSuccess = true;
                }
                else{
                    testSuccess = false;
                }
            }
            if (testSuccess == true) //Nutzerausgabe 
                {
                    System.Console.WriteLine("Sortieren-Test erfolgreich!");
                }else
                {
                    System.Console.WriteLine("Sortieren-Test nicht erfolgreich!");
                }
        }
    }
    class Program
    {    
        static void Main(string[] args)
        {
            // Dateiname von csv
            string pathBaeumeCsv = @"./csv/baeume.csv"; //./csv/baeume.csv
            //Anzahl der Bäume in Csv
            int anzahlInListe = 49885;

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

                    { //neuer Baum zu Liste hinzufügen
                        BaumListe.Add(new Baeume(baeumeAsCsvString[i]));
                    }
                }
                catch (IndexOutOfRangeException) // wird ausgegeben, falls anzahlInListe nicht korrekt angegeben
                {
                    System.Console.WriteLine("Out Of Range");
                }
                
                //Hinweißkommentar
                System.Console.WriteLine("\n##### Hier folgt die sortierte Liste: #####\n");
                
                //Aufruf sort-function
                BaumListe = Sortieren.quicksortPflanzdatum(BaumListe);

                int bc = 1; //Nummer zur Kontrolle der Ausgabe
                foreach (Baeume aBaum in BaumListe)
                {
                    if (bc>50){break;} //Abbrechen der Auflistung nach den ersten 50 Elementen
                    System.Console.WriteLine("______________________\nBaumdaten:");
                    System.Console.WriteLine("### {0} ###",bc); //Ausgabenummer
                    bc ++;
                    System.Console.WriteLine(aBaum.ToString()); //Ausgabe einzelner Baum
                }
            }
            // Test ob Sortieren in aufsteigender Reihenfolge funktioniert
            Testen tests = new Testen();
            tests.CheckIfAscending(BaumListe);
            
            // Fix object reference for non static field or method
            Umkreis neuerUmkreis = new Umkreis();
            neuerUmkreis.Rechnen();//Aufruf Funktion Umkreis/Mittelpunkt brechnen
            //Ausgabe Mittlepunkt und Umkreis
            System.Console.WriteLine("______________________\n----------------------\nMittelpunkt: {0}|{1} \nUmkreis: {2} \n",Umkreis.midwe,Umkreis.midns,Umkreis.abstand);
            neuerUmkreis.OutputCheck(); //Testing   
            tests.CheckAbstand(Umkreis.abstand);
        }
    }
}//Hier ist das Ende eines wunderschönen Programmcodes.
