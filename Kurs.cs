using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaStranihJezika
{
    internal class Kurs
    {
        public static int brojacId = 0;
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int BrojUcenika { get; set; }
        public bool Aktivan { get; set; }
        public string Jezik { get; set; }
        public Kurs (string naziv, int brojUcenika, bool aktivan, string jezik, int id = -1)
        {
            if (id == -1)
            {
                this.Id = brojacId++;
            }
            else if (id >= brojacId)
            {
                this.Id = id;
                brojacId = ++id;
            }
            Naziv = naziv;
            BrojUcenika = brojUcenika;
            Aktivan = aktivan;
            Jezik = jezik;
        }
        public string SkracenPrikaz()
        {
            return $" {Id} | {Naziv}";
        }
        public override string ToString()
        {
            string resAktivan = Aktivan ? "Da" : "Ne";
            return $"\n{Id} {Naziv}\n" +
                $"Broj ucenika: {BrojUcenika}\n" +
                $"Aktivan: {resAktivan}\n" +
                $"Strani jezik: {Jezik}\n" +
                $"{new string('-', 30)}\n";
        }
    }
}
