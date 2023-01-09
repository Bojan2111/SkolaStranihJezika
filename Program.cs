using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using KonzolniMeni;

namespace SkolaStranihJezika
{
    internal class Program
    {
        /* Za deo zadatka sa varijabilnim maksimumom polaznika za svaki kurs ponaosob, sve ostaje isto, samo se dodaje
         * polje int MaksimumPolaznika u klasu Kurs i inicijalizuje se vrednost prilikom ucitavanja podataka iz
         * tabele kursevi_max iz Skola database. Logika odredjivanja dostupnih kurseva ostaje ista, jer se kao
         * granicnik koristi promenljiva - maksimumPolaznika.
         * Navedeni nacin nisam implementirao ovde, ali sam kreirao tabelu u bazi podataka za rad.
         */
        static void Main(string[] args)
        {
            DAO.UnosSvegaUListu();
            DAO.UnosKursevaUcenika();
            Meni meni = new Meni();
            meni.DodajOpciju(DAO.IspisKurseva, "Pregled svih kurseva");
            meni.DodajOpciju(DAO.NoviUcenik, "Dodavanje novih učenika");
            meni.DodajOpciju(DAO.Ubacivanje, "Ubacivanje učenika na kurs");
            meni.DodajOpciju(DAO.IspisKursevaPoJeziku, "Ispis kurseva na kojima se uci odredjen strani jezik");
            meni.DodajOpciju(DAO.IzveziPDF, "Izvezi podatke o kursu u PDF fajl");
            meni.Pokreni();
        }
    }
}
