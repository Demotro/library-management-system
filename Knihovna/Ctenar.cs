using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Knihovna
{
    [Serializable()]
    public class Ctenar : INotifyPropertyChanged
    {
        //privatni atributy pro ulozeni dat ctenare
        private string _jmeno;
        private string _prijmeni;
        private string _email;
        private int _telefonniCislo;

        //vlastnosti, ktere ctou a zapisuji privatni atributy
        public string Jmeno
        {
            get => _jmeno;
            set
            {
                if (_jmeno != value) // kdyz se zmeni hodnota, tak se aktualizuje atribut
                { _jmeno = value; NotifyPropertyChanged(); }
            }
        }
        public string Prijmeni
        {
            get => _prijmeni;
            set
            { 
                if (_prijmeni != value)
                { _prijmeni = value; NotifyPropertyChanged(); }
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                { _email = value; NotifyPropertyChanged(); }
            }
        }
        public int TelefonniCislo
        {
            get => _telefonniCislo;
            set
            {
                if (_telefonniCislo != value)
                { _telefonniCislo = value; NotifyPropertyChanged(); }
            }
        }

        //kolekce knih, ktere ma ctenar vypujcene
        public BindingList<Kniha> Vypujcene { get; private set; } = new BindingList<Kniha>();

        //kolekce knih, ktere ma ctenar rezervovane
        public BindingList<Kniha> Rezervovano { get; private set; } = new BindingList<Kniha>();

        //konstruktor pro prazdnou instanci ctenare, potrebny pro serializaci
        public Ctenar() { }

        //konstruktor pro vytvoreni instance ctenare
        public Ctenar(string jmeno, string prijmeni, string email, int telefonniCislo)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            Email = email;
            TelefonniCislo = telefonniCislo;
        }

        //kdyz se zmeni vlastnost ctenare, tak tahle metoda aktualizuje user interface toho ctenare
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {   //udalost upozorni user interface na zmenu vlastnosti
            PropertyChanged?.Invoke(propertyName, new PropertyChangedEventArgs(propertyName));
        }
        //udalost informuje user interface o zmene vlastnosti
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}