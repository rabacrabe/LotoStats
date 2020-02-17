using System;
using System.Collections.Generic;
using System.IO;

namespace LotoStats.Models
{
     class Tirage
    {
        public string _jour { get; set; }
        public string _date { get; set; }
        public string _n1 { get; set; }
        public string _n2 { get; set; }
        public string _n3 { get; set; }
        public string _n4 { get; set; }
        public string _n5 { get; set; }
        public string _cmpl { get; set; }
        public string _nbGagnants { get; set; }
        public string _jackpotEUR { get; set; }

        public void setAllFromList(string[] lstElement) {
            _jour = lstElement[0];
            _date = lstElement[1];
            _n1 = lstElement[2];
            _n2 = lstElement[3];
            _n3 = lstElement[4];
            _n4 = lstElement[5];
            _n5 = lstElement[6];
            _cmpl = lstElement[7];
            _nbGagnants = lstElement[8];
            _jackpotEUR = lstElement[9];
        }
    }

    class Position
    {
        private string _name;
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        private List<Boule> _lstBoule;
        public List<Boule> LstBoule
        {
            get => _lstBoule;
            set => _lstBoule = value;
        }

        public Position(string name)
        {
            Name = name;
            _lstBoule = new List<Boule>();
        }

        public void AddBoule(string valeure, int occurence, double pct, int position)
        {
            Boule newBoule = new Boule(valeure, occurence, pct, position);
            _lstBoule.Add(newBoule);
        }

        public Boule getBoule(string number)
        {
            foreach (Boule boule in LstBoule)
            {
                if (number == boule.Valeure)
                {
                    return boule;
                }
            }
            return null;
        }

        public Boule getBouleFromPosition(int position)
        {
            foreach (Boule boule in LstBoule)
            {
                if (position == boule.Pos)
                {
                    return boule;
                }
            }
            return null;
        }
    }

    class Boule
    {
        private string _valeure;
        public string Valeure
        {
            get=>_valeure;
            set=>_valeure=value;
        }
        private int _occurence{ get; set; }
        public int Occurence
        {
            get=>_occurence;
            set=>_occurence=value;
        }
        private double _pct{ get; set; }
        public double Pct
        {
            get=>_pct;
            set=>_pct=value;
        }
        private int _pos{ get; set; }
        public int Pos
        {
            get=>_pos;
            set=>_pos=value;
        }
        public Boule(string valeure, int occurence, double pct, int position)
        {
            _valeure = valeure;
            _occurence = occurence;
            _pct = pct;
            _pos = position;
        }

        public string print()
        {
            return $"Valeure={_valeure}, Occurence={_occurence}, Position={_pos}, Pourcentage={_pct}";
        }
    }
}