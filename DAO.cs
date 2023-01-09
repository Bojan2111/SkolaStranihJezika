using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaStranihJezika
{
    internal class DAO
    {
        private static readonly int maksimumUcenika = 8;
        public static List<Kurs> listaKurseva = new List<Kurs>();
        public static List<Ucenik> listaUcenika = new List<Ucenik>();
        public static Dictionary<int, List<int>> kurseviUcenika = new Dictionary<int, List<int>>();

        public static void IspisKurseva()
        {
            foreach (Kurs k in listaKurseva)
            {
                Console.WriteLine(k);
            }
        }

        public static void IspisUcenika()
        {
            foreach (Ucenik u in listaUcenika)
            {
                Console.WriteLine(u);
            }
        }

        public static void NoviUcenik()
        {
            Console.WriteLine("Unesite ime ucenika:");
            string ime = Console.ReadLine();
            Console.WriteLine("Unesite prezime ucenika:");
            string prezime = Console.ReadLine();

            listaUcenika.Add(new Ucenik(ime, prezime));
            SqlConnection conn = new SqlConnection("Server =.\\SQLEXPRESS; Database = Skola; Integrated Security=true;");
            conn.Open();
            string insertString = "INSERT INTO ucenici " +
                "(ime, prezime) " +
                "VALUES (@ime, @prezime);";
            SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.Parameters.AddWithValue("@ime", ime);
            cmd.Parameters.AddWithValue("@prezime", prezime);
            cmd.ExecuteNonQuery();
            conn.Close();
            Console.WriteLine("Ucenik uspesno dodat!");
        }

        public static void Ubacivanje()
        {
            foreach (Ucenik u in listaUcenika)
            {
                Console.WriteLine(u.SkraceniPrikaz());
            }
            Console.WriteLine("\nUnesite ID ucenika kojeg zelite ubaciti u kurs:");
            int idUcenika = int.Parse(Console.ReadLine());
            Console.WriteLine("\nDostupni kursevi:");
            foreach (Kurs k in listaKurseva)
            {
                if (k.Aktivan && k.BrojUcenika < maksimumUcenika)
                {
                    Console.WriteLine(k.SkracenPrikaz());
                }
            }
            Console.WriteLine("Unesite ID kursa u koji zelite ubaciti ucenika:");
            int idKursa = int.Parse(Console.ReadLine());
            // unos ucenika i kursa u dict, unos kursa u listu kurseva ucenika, unos u odg. tabele u sql db
            if (kurseviUcenika.ContainsKey(idUcenika))
            {
                kurseviUcenika[idUcenika].Add(idKursa);
            }
            else
            {
                kurseviUcenika[idUcenika] = new List<int> { idKursa };
            }
            foreach (Ucenik uc in listaUcenika)
            {
                if (uc.Id == idUcenika)
                {
                    foreach (Kurs ku in listaKurseva)
                    {
                        if (ku.Id == idKursa)
                        {
                            uc.Kursevi.Add(ku);
                            break;
                        }
                    }
                    break;
                }
            }
            SqlConnection conn = new SqlConnection("Server =.\\SQLEXPRESS; Database = Skola; Integrated Security=true;");
            conn.Open();
            string insertString = "INSERT INTO kursevi_ucenika " +
                "(id_ucenika, id_kursa) " +
                "VALUES (@id_ucenika, @id_kursa);";
            SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.Parameters.AddWithValue("@id_ucenika", idUcenika);
            cmd.Parameters.AddWithValue("@id_kursa", idKursa);
            cmd.ExecuteNonQuery();
            conn.Close();
            Console.WriteLine("Ucenik uspesno ubacen na kurs!");
        }
        public static void UnosSvegaUListu()
        {
            SqlConnection conn = new SqlConnection("Server =.\\SQLEXPRESS; Database = Skola; Integrated Security=true;");
            conn.Open();
            SqlCommand cmd1 = new SqlCommand("select * from kursevi", conn);
            SqlDataReader rdr1 = cmd1.ExecuteReader();

            while (rdr1.Read())
            {
                int id = int.Parse(rdr1["id"].ToString());
                string naziv = rdr1["naziv"].ToString();
                int brojUcenika = int.Parse(rdr1["broj_ucenika"].ToString());
                bool aktivan = (bool)rdr1["aktivan"];
                string jezik = rdr1["jezik"].ToString();

                listaKurseva.Add(new Kurs(naziv, brojUcenika, aktivan, jezik, id));
            }
            rdr1.Close();
            SqlCommand cmd2 = new SqlCommand("select * from ucenici", conn);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            while (rdr2.Read())
            {
                int id = int.Parse(rdr2["id"].ToString());
                string ime = rdr2["ime"].ToString();
                string prezime = rdr2["prezime"].ToString();

                listaUcenika.Add(new Ucenik(ime, prezime, id));
            }
            rdr2.Close();
            SqlCommand cmd3 = new SqlCommand("select * from kursevi_ucenika", conn);
            SqlDataReader rdr3 = cmd3.ExecuteReader();
            while (rdr3.Read())
            {
                //int id = int.Parse(rdr3["id"].ToString());
                int id_ucenika = int.Parse(rdr3["id_ucenika"].ToString());
                int id_kursa = int.Parse(rdr3["id_kursa"].ToString());
                if (kurseviUcenika.ContainsKey(id_ucenika))
                {
                    kurseviUcenika[id_ucenika].Add(id_kursa);
                }
                else
                {
                    kurseviUcenika[id_ucenika] = new List<int> { id_kursa };
                }

            }

            rdr3.Close();
            conn.Close();
        }
        public static void UnosKursevaUcenika()
        {
            foreach (Ucenik u in listaUcenika)
            {
                int idUcenika = u.Id;
                List<Kurs> tempKursevi = new List<Kurs>();
                if (kurseviUcenika.ContainsKey(idUcenika))
                {
                    foreach (int i in kurseviUcenika[idUcenika])
                    {
                        foreach (Kurs k in listaKurseva)
                        {
                            if (k.Id == i)
                            {
                                tempKursevi.Add(k);
                                break;
                            }
                        }
                    }
                }
                u.Kursevi = tempKursevi;
            }
        }

        internal static void IspisKursevaPoJeziku()
        {
            Console.WriteLine("Unesite jezik kursa:");
            string jezik = Console.ReadLine();
            foreach (Kurs k in listaKurseva)
            {
                if (k.Jezik == jezik)
                {
                    Console.WriteLine(k);
                }
            }
        }
        public static void IzveziPDF()
        {
            Console.WriteLine("Unesite ID kursa za koji zelite izvesti podatke u PDF:");
            int kursID = int.Parse(Console.ReadLine());
            Console.WriteLine(IspisPodatakaOKursu(kursID));
            // Iz nekog razloga, verovatno sto u ovom solution koristim .NET 5, MigraDoc ne radi kao ocekivano.
            // Neke funkcije potrebne u ovom solution zahtevaju minimum c# v.8, tako da ne mogu koristiti npr 4.7.2
            // Pretrazio sam moguca resenja problema, skoro svi ukazuju na System.Runtime, ali ne mogu ga pronaci.
            // Kod koji je zakomentiran bi trebalo da radi, sveden je na konzolni ispis podataka o kursu.

            //var document = CreateDocument(IspisPodatakaOKursu(kursID));
            //const bool unicode = true;

            //var pdfRenderer = new PdfDocumentRenderer(unicode);

            //pdfRenderer.Document = document;

            //pdfRenderer.RenderDocument();

            //string filename = $"Kurs-{kursID}.pdf";
            //pdfRenderer.PdfDocument.Save(filename);
            //Process.Start(filename);
        }
        //static Document CreateDocument(string ispis)
        //{
        //    var document = new Document();

        //    document.Styles[StyleNames.Normal].Font.Name = "Lucida Sans";

        //    var section = document.AddSection();

        //    var paragraph = section.AddParagraph();

        //    paragraph.Format.Font.Color = Colors.DarkBlue;

        //    paragraph.AddFormattedText(ispis, TextFormat.Bold);

        //    var footer = section.Footers.Primary;

        //    paragraph = footer.AddParagraph();
        //    paragraph.Add(new DateField() { Format = "yyyy/MM/dd HH:mm:ss" });
        //    paragraph.Format.Alignment = ParagraphAlignment.Center;

        //    return document;
        //}
        internal static string IspisPodatakaOKursu(int kursId)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Kurs k in listaKurseva)
            {
                if (k.Id == kursId)
                {
                    sb.Append(k);
                    sb.Append("\n");
                    break;
                }
            }
            sb.AppendLine("Ucenici koji pohadjaju ovaj kurs:");
            foreach (KeyValuePair<int, List<int>> kurs in kurseviUcenika)
            {
                if (kurs.Value.Contains(kursId))
                {
                    foreach (Ucenik u in listaUcenika)
                    {
                        if (u.Id == kurs.Key)
                        {
                            sb.AppendLine($"{u.Ime} {u.Prezime}");
                        }
                    }
                }
            }
            return sb.ToString();
        }
    }
}
