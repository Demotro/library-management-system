using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knihovna
{
    [Serializable]
    public class Ctenar
    {
        //privatni atributy pro ulozeni dat ctenare
        private int _id;
        private string _jmeno = string.Empty;
        private string _prijmeni = string.Empty;
        private string _telefonniCislo = string.Empty;
        private string _email = string.Empty;

        //vlastnosti, ktere ctou a zapisuji privatni atributy
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Jmeno
        {
            get => _jmeno;
            set => _jmeno = value;
        }

        public string Prijmeni
        {
            get => _prijmeni;
            set => _prijmeni = value;
        }

        public string TelefonniCislo
        {
            get => _telefonniCislo;
            set => _telefonniCislo = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        //seznam knih, ktere ma ctenar vypujcene
        public BindingList<Kniha> Vypujcene { get; set; } = new BindingList<Kniha>();

        //seznam knih, ktere ma ctenar rezervovane
        public BindingList<Kniha> Rezervovano { get; set; } = new BindingList<Kniha>();

        //konstruktor prazdne instance ctenare, potrebny pro serializaci
        public Ctenar() { }

        //konstruktor pro vytvoreni instance ctenare
        public Ctenar(string jmeno, string prijmeni, string telefonniCislo, string email)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            TelefonniCislo = telefonniCislo;
            Email = email;
        }

        public override string ToString()
        {
            return $"{Jmeno} {Prijmeni}";
        }
    }
}