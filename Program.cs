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
            
            // Der Array wird von 0 ausgehend abgelaufen und die Werte werden in der jeweiligen Reihenfolge (bzgl. der Reihenfolge der Kategorien 
            //im Tabellenkopf aka Index 0 der beim Einlesen der Daten (siehe // Daten in Liste schreiben) in der Main Funktion übersprungen wird) gespeichert
            try
            {   
                X_Koordinate = Convert.ToDouble(csvEntries[0]);
                Y_Koordinate = Convert.ToDouble(csvEntries[1]);
                ObjektID = Convert.ToInt16(csvEntries[2]);
                ID = Convert.ToInt32(csvEntries[3]);
                Objektschluessel = csvEntries[4];
                Baumnummer = csvEntries[5];
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

                for(int i = 0; i >= 49 ;i++)
                { //Array sortiert nach Norden
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

                for(int i = 0; i >= 49 ;i++)
                { 
                    //Array sortiert nach Westen
                    Wests [i]= list[0].X_Koordinate;
                }
                
                return sorted;
            }

            //Abstandsberechnug mit Hilfe des Satz von Pythagoras
            abstand =  0.0;
            for(int i = 0; i >= 49; i++)
            {
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
        {       
            bool westtree_bool = false;
            bool easttree_bool  = false;
            bool nordtree_bool  = false;
            bool southtree_bool  = false;
            bool abstand_bool = false;

            // Wenn der westlichste Baum Koordinaten übergeben bekommt, sind diese ungleich 0, also ist der Test true
            if (westtree != 0)
            {
                westtree_bool = true;
            }

            // Analog für Ost
            if (easttree != 0)
            {
                easttree_bool = true;
            }

            // Analog für Nord
            if (nordtree != 0)
            {
                nordtree_bool = true;
            }

            // Analog für Süd
            if (southtree != 0)
            {
                southtree_bool = true;
            }

            if (abstand != 0)
            {
                abstand_bool = true;
            }
            //Testen und überprüfen der Ausgabe
            System.Console.WriteLine("\n======================");
            System.Console.WriteLine("Koordinaten der Extrema-Bäume werden überprüft...\n");
            System.Console.WriteLine("Westtree: {0}",westtree_bool);
            System.Console.WriteLine("Easttree: {0}",easttree_bool);
            System.Console.WriteLine("Northtree: {0}",nordtree_bool);
            System.Console.WriteLine("Southtree: {0}",southtree_bool);
            System.Console.WriteLine("Abstand: {0}",abstand_bool);
            System.Console.WriteLine("======================");
        }
    }

    class Testen
    {
        public bool testSuccess {get; set;}
        // Testfunktion für den Abstand - falls er ungleich 0 ist, wird er richtig ausgerechnet. Zur Zeit ist ist das Ergebnis immer 0
        public void CheckAbstand (double abstand)
        {
            if (abstand != 0)//Testausgabe für Erfolg des Abstandtests 
            {   
                System.Console.WriteLine("\n======================");
                System.Console.WriteLine("Abstand-Test wird durchgeführt...");
                System.Console.WriteLine("Abstand-Test bestanden!"); //Ergebnis wenn wir was können
                System.Console.WriteLine("======================");
            }else
            {
                System.Console.WriteLine("\n======================");
                System.Console.WriteLine("Abstand-Test wird durchgeführt...");
                System.Console.WriteLine("Abstand-Test fehlgeschlagen!"); //Ergebnis wenn der Computer, nicht das macht was wir wollen
                System.Console.WriteLine("======================");
            }
        }
        // Testen ob Sortieren erfolgreich
        public void CheckIfAscending (List<Baeume> list)
        {
            // Es reicht hier nur die ersten 50 Einträge zu checken, da diese hinreichend sind um eine Aussage über die Funktionalität des Sortieralgorithmus zu treffen
            for (int i = 0; i < 49; i++)
            {
                if (list[i].Pflanzdatum <= list[i+1].Pflanzdatum)
                { 
                    //Kontrolle ob Pflanzdatum aufsteigend sortiert ist
                    testSuccess = true;
                }
                else{
                    testSuccess = false;
                }
            }
            if (testSuccess == true) //Testausgabe für Erfolg des Sortierentests 
            {   
                System.Console.WriteLine("\n======================");
                System.Console.WriteLine("Sortieren-Test wird durchgeführt...");
                System.Console.WriteLine("Sortieren-Test erfolgreich!");
                System.Console.WriteLine("======================");
            }else
            {
                System.Console.WriteLine("\n======================");
                System.Console.WriteLine("Sortieren-Test wird durchgeführt...");
                System.Console.WriteLine("Sortieren-Test nicht erfolgreich!");
                System.Console.WriteLine("======================");
            }
        }
    }

    class printCsv
    {
        public void print(List <Baeume> sortierteBaeume)
        {
            // Durch eine If-Abfrage kann das Program fehlerfrei laufen, auch wenn es schonmal gestartet wurde und somit die csv schon existiert
            if (File.Exists("SortiertNachPflanzdatum.csv")==false)
            {
                File.WriteAllLines("./csv/SortiertNachPflanzdatum.csv", sortierteBaeume.Select(x => string.Join(",", x)));    
            }else
            {
                File.Delete("./csv/SortiertNachPflanzdatum.csv");
                File.WriteAllLines("./csv/SortiertNachPflanzdatum.csv", sortierteBaeume.Select(x => string.Join(",", x)));
            }
        }
    }
    
    class Program
    {    
        static void Main(string[] args)
        {
            var eingabe = 0;
            System.Console.WriteLine("\nWillkommen im Projekt der PlantPowerdProgrammers!\n");
            System.Console.WriteLine("Geben Sie eine der folgenden Menüoptionen ein!");
            System.Console.WriteLine("=================================================");
            System.Console.WriteLine("(1) Unsortierte Liste ausgeben");
            System.Console.WriteLine("(2) nach Pflanzdatum sortierte Liste ausgeben");
            System.Console.WriteLine("(3) nach Pflanzdatum sortierte CSV-Datei ausgeben");
            System.Console.WriteLine("(4) Tests ausgeben");
            System.Console.WriteLine("(5) Programm schließen");
            System.Console.WriteLine("=================================================");
            
            // Benutzereingabe einlesen und auf eingabe speichern
            try
            {   
                ConsoleKeyInfo input = Console.ReadKey();
                if (char.IsDigit(input.KeyChar))
                {
                    eingabe = int.Parse(input.KeyChar.ToString());
                }else
                {
                    eingabe = 0;
                }
            }
            catch (InvalidOperationException) //wenn keine Zahl eingegeben wird, dann schließt das Programm
            {
                
            }
            

            // Dateiname von csv
            string pathBaeumeCsv = @"./csv/baeume.csv";

            // Liste von Bäumen erstellen
            List<Baeume> BaumListe = new List<Baeume>();
            
            // BaumListe mit Einträgen aus der csv füllen
            if(File.Exists(pathBaeumeCsv))
            {
                // Zeile lesen
                string[] baeumeAsCsvString = File.ReadAllLines(pathBaeumeCsv);

                int stringLength = baeumeAsCsvString.GetLength(0);
                
                // Daten in Liste schreiben
                try
                {
                    for (int i = 1; i <= 49886; i++)

                    { //neuer Baum zu Liste hinzufügen
                        BaumListe.Add(new Baeume(baeumeAsCsvString[i]));
                    }
                }
                catch (IndexOutOfRangeException) // wird ausgegeben, falls anzahlInListe nicht korrekt angegeben
                {
                    // System.Console.WriteLine("Out Of Range");
                }
                
                // Falls Menüpunkt 1 gewählt wurde wird die unsortierte Liste ausgegeben
                if (eingabe == 1)
                {
                    int bc = 1;
                    
                    //Hinweißkommentar
                    System.Console.WriteLine("\n##### UNSORTIERTE LISTE #####\n");
                    foreach (Baeume aBaum in BaumListe)
                        {

                            if (bc>50){break;} //Abbrechen der Auflistung nach den ersten 50 Elementen
                            System.Console.WriteLine("______________________\nBaumdaten:");
                            System.Console.WriteLine("### {0} ###",bc); //Ausgabenummer
                            bc ++;
                            System.Console.WriteLine(aBaum.ToString()); //Ausgabe einzelner Baum
                        }
                }else
                {   
                    // Analog für Menüpunkt 2 und der sortierten Liste
                    if (eingabe == 2)
                    {
                        //Hinweißkommentar
                        System.Console.WriteLine("\n##### SORTIERTE LISTE #####\n");

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
                }
            }
            
            
            // Fix object reference for non static field or method
            Umkreis neuerUmkreis = new Umkreis();
            neuerUmkreis.Rechnen();//Aufruf Funktion Umkreis/Mittelpunkt brechnen

            // Falls die Tests ausgegeben werden sollen
            if (eingabe == 4)
            {  
                // Tests
                System.Console.WriteLine("\n======================");
                System.Console.WriteLine("Ab hier beginnen die Tests!");
                System.Console.WriteLine("======================");

                //Ausgabe Mittlepunkt und Umkreis
                System.Console.WriteLine("\n======================");
                System.Console.WriteLine("Mittelpunkt und Umkreis werden getestet...\n");
                System.Console.WriteLine("Mittelpunkt: ({0}|{1})",Umkreis.midwe,Umkreis.midns);
                System.Console.WriteLine("Umkreis: {0}",Umkreis.abstand);
                System.Console.WriteLine("======================");

                // Testen Koordinaten und Abstand mit Output-Check
                neuerUmkreis.OutputCheck(); //Testing   

                // Klasse Testen instanziieren
                Testen tests = new Testen();

                // Test ob Sortieren in aufsteigender Reihenfolge funktioniert
                tests.CheckIfAscending(BaumListe);

                // Test ob Abstand zum Mittelpunkt korrekt ausgerechnet wird
                tests.CheckAbstand(Umkreis.abstand);
            }
            
            // Falls CSV Dateien ausgegeben werden sollen
            if (eingabe == 3)
            {   
                // Hinweiskommentar
                System.Console.WriteLine("\nCSV-Datei wird erstellt und ausgegeben...");
                // CSV 'SortiertNachPflanzdatum.csv' wird erstellt
                printCsv neueCsv = new printCsv();
                neueCsv.print(BaumListe);
            }

            // Falls eine 5 eingegeben wurde und sich das Programm einfach schließen soll
            if (eingabe ==5)
            {
                System.Console.WriteLine("\nProgramm wird geschlossen...");
            }      
        }
    }
}//Hier ist das Ende eines wunderschönen Programmcodes.

