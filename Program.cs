using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LotoStats.Models;


namespace LotoStats
{
    class Program
    {
        Dictionary<string, Position> mapFinale = new Dictionary<string, Position>();

        static void Main(string[] args)
        {
            //Console.Write("\nEnter CSV filepath:\t");
            //string filePath = Console.ReadLine();
            string filePath = "C:\\Users\\gtheurillat\\Desktop\\loto_tirages.csv";

            Program p = new Program();
            List<Tirage> lstTirages = p.loadCsvFile(filePath);
            Console.WriteLine($"Read {lstTirages.Count} tirages");

            p.calculStats(lstTirages);

            p.Actions();
        }

        public void Actions()
        {
            Console.Write("\n1=Search 2=Generate:");
            string choice = Console.ReadLine();

            switch(choice) 
            {
            case "1":
                Console.Write("Tirage:");
                string tirage = Console.ReadLine();
                Search(tirage);
                Actions();
                break;
            case "2":
                Console.Write("Position max:");
                string pos_max = Console.ReadLine();
                Generate(Int32.Parse(pos_max));
                Actions();
                break;
            default:
                // code block
                break;
            }

            
        }

        public void Search(string tirage)
        {
            string[] lstTirage = tirage.Split(" ");

            int i = 0;
            foreach(string pos_name in new string[] {"N1","N2","N3","N4","N5","CMPL"})
            {
                Boule newBoule = mapFinale[pos_name].getBoule(lstTirage[i]);
                Console.WriteLine("{3}={0} ({1} %) \t\t {2}", lstTirage[i], newBoule.Pct, newBoule.Pos, pos_name);
                i++;
            }
        }

        public void Generate(int pos_max)
        {
            List<string> newTirage = new List<string>();
            foreach(string pos_name in new string[] {"N1","N2","N3","N4","N5","CMPL"})
            {
                //Console.WriteLine(pos_name);

                Boule[] lstBoules = new Boule[pos_max];
                for (int i = 1; i <= pos_max; i++)
                {
                    Boule newBoule = mapFinale[pos_name].getBouleFromPosition(i);
                   // Console.WriteLine($"RECHERCHE BOULE FOR POS {i} -> {newBoule.print()}");
                    lstBoules[i-1] = newBoule;
                }
                //au hazard maintenant
                var rand = new Random();

                int aletatoire = rand.Next(pos_max-1);
                string valeurAleatoire = lstBoules[aletatoire].Valeure;
                while (newTirage.Contains(valeurAleatoire))
                {
                    aletatoire = rand.Next(pos_max-1);
                    valeurAleatoire = lstBoules[aletatoire].Valeure;
                }
                newTirage.Add(valeurAleatoire);
                //Console.WriteLine(lstBoules.Count());
                //Console.WriteLine("FOR {0} GET POSITION {1}", pos_name, aletatoire+1);
            }

            foreach (string bouleAleatoire in newTirage)
            {
                Console.Write(bouleAleatoire + " ");
            }
        }

        public List<Tirage> loadCsvFile(string filePath)
        {
            var reader = new StreamReader(File.OpenRead(filePath));
            List<Tirage> lstTirages = new List<Tirage>();
            bool firstLine = true;
            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                
                if (firstLine == true) 
                {
                    firstLine = false;
                    continue;
                }

                string[] lineElements = line.Split(";");
                if (lineElements.Length > 0) 
                {
                    //Console.WriteLine(line);
                    Tirage newTirage = new Tirage() {
                        _jour = lineElements[0],
                        _date = lineElements[1],
                        _n1 = lineElements[2],
                        _n2 = lineElements[3],
                        _n3 = lineElements[4],
                        _n4 = lineElements[5],
                        _n5 = lineElements[6],
                        _cmpl = lineElements[7],
                        _nbGagnants = lineElements[8],
                        _jackpotEUR = lineElements[9],
                    };
                    //newTirage.setAllFromList(lineElements);
                    lstTirages.Add(newTirage);
                }
                
            }
            return lstTirages;
        }

        public void calculStats(List<Tirage> lstTirage)
        {
            Dictionary<string, List<string>> mapTirages = new Dictionary<string, List<string>>() {
                {"N1", new List<string>()},
                {"N2", new List<string>()},
                {"N3", new List<string>()},
                {"N4", new List<string>()},
                {"N5", new List<string>()},
                {"CMPL", new List<string>()},
            };

            foreach (Tirage tirage in lstTirage)
            {
                mapTirages["N1"].Add(tirage._n1);
                mapTirages["N2"].Add(tirage._n2);
                mapTirages["N3"].Add(tirage._n3);
                mapTirages["N4"].Add(tirage._n4);
                mapTirages["N5"].Add(tirage._n5);
                mapTirages["CMPL"].Add(tirage._cmpl);
            }

            
            

            foreach( KeyValuePair<string, List<string>> kvp in mapTirages)
            {
                Dictionary<string, int> mapTirageUnit = new Dictionary<string, int>();

                foreach(string tirageNumber in kvp.Value)
                {
                    if (!mapTirageUnit.ContainsKey(tirageNumber))
                    {
                        mapTirageUnit.Add(tirageNumber, 0);
                    }
                    mapTirageUnit[tirageNumber] += 1;
                }

                Position newPos = new Position(kvp.Key);
                mapFinale.Add(kvp.Key, newPos);

                Console.WriteLine("Boule {0}", kvp.Key);

                var items = from pair in mapTirageUnit
                    orderby pair.Value descending
                    select pair;

                // Display results.
                int position = 1;
                foreach (KeyValuePair<string, int> pair in items)
                {
                    double pct = (pair.Value * 100.0) / lstTirage.Count;
                    Console.WriteLine("\t {0}: {1} ({2} %) \t {3}", pair.Key, pair.Value, Math.Round(pct,3), position);

                    mapFinale[kvp.Key].AddBoule(pair.Key, pair.Value, Math.Round(pct,3), position);
                    position += 1;
                }

               
                //Console.WriteLine("\t {0}, {1}", lstSorted[0], mapTirageUnit[lstSorted[0]]);
               

            }
        }
    }

   
}
