using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaStranihJezika
{
    internal class Ucenik
    {
        public static int brojacId = 0;
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public List<Kurs> Kursevi { get; set; }
        public Ucenik(string ime, string prezime, int id = -1)
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
            Ime = ime;
            Prezime = prezime;
            Kursevi = new List<Kurs>();
        }
        public string SkraceniPrikaz()
        {
            return $" {Id} | {Ime} {Prezime}";
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Kurs kurs in Kursevi)
            {
                sb.Append($"{kurs.Naziv}\n");
            }
            return $"| {Id} | {Ime} {Prezime} pohadja sledece kurseve:\n{sb}";
        }
    }
}
