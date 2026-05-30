using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace Knihovna
{
    public static class Databaze
    {   //vytvoreni nove kolekce ctenari a knihy, pro pridabi a odebrani instanci ctenaru a knih
        //aktualizuje datove zobrazeni v user interface
        public static BindingList<Ctenar> Ctenari { get; private set; } = new BindingList<Ctenar>();
        public static BindingList<Kniha> Knihy { get; private set; } = [];
        static Databaze()//konstruktor, nacita data ze souboru
        {
            Deserializuj();
        }
        public static bool Vypujcit(object cO, object kO)//metoda pro vypujceni knihy ctenari
        {
            var ctenar = (Ctenar)cO;
            var kniha = (Kniha)kO;

            if (kniha.Dostupnost == false)
            {
                MessageBox.Show("Kniha je vypůjčená, nemůžeš si ji vypůjčit.");
                return false;
            }

            var rezervace = ctenar.Rezervovano.FirstOrDefault(k => k.ISBN == kniha.ISBN);

            if (rezervace != null)
            {
                ctenar.Rezervovano.Remove(rezervace);
            }

            ctenar.Vypujcene.Add(kniha);
            kniha.Dostupnost = false;

            return true;
        }
        public static void Vratit(object cO, object kO)//metoda pro vraceni vypujcene knihy
        {
            var ctenar = (Ctenar)cO;
            var kniha = (Kniha)kO;
            ctenar.Vypujcene.Remove(kniha);
            kniha.Dostupnost = true;
        }
        public static bool Rezervovat(object cO, object kO)//metoda pro rezervaci
        {
            var ctenar = (Ctenar)cO;
            var kniha = (Kniha)kO;

            if (kniha.Dostupnost == true)
            {
                MessageBox.Show("Kniha je dostupná, můžeš si ji rovnou vypůjčit.");
                return false;
            }

            if (ctenar.Vypujcene.Any(k => k.ISBN == kniha.ISBN))
            {
                MessageBox.Show("Knihu máš už vypůjčenou, nemůžeš ji rezervovat!");
                return false;
            }

            if (ctenar.Rezervovano.Any(k => k.ISBN == kniha.ISBN))
            {
                MessageBox.Show("Knihu už máš rezervovanou!");
                return false;
            }

            ctenar.Rezervovano.Add(kniha);
            return true;
        }
        public static void Zrusit(object cO, object kO)//metoda pro zruseni rezervace
        {
            var ctenar = (Ctenar)cO;
            var kniha = (Kniha)kO;
            ctenar.Rezervovano.Remove(kniha);
        }
        public static bool EditovanaKniha(Kniha kniha)//metoda pro kontrolu jestli muze byt kniha editovana
        {
            foreach (var ctenar in Ctenari)
            {
                //kontrola, ze nemuze byt kniha editovana, kdyz je pujcena nebo rezervovana
                if (ctenar.Vypujcene.Contains(kniha) || ctenar.Rezervovano.Contains(kniha))
                { MessageBox.Show("Knihu nemůžeš editovat, protože je vypůjčená nebo rezervovaná."); return false; }
            }
            return true;
        }
        public static bool SmazatKnihu(Kniha kniha)//metoda pro smazani knihy z databaze
        {
            if (Knihy.Count == 1)
            { MessageBox.Show("Nemůžeš smazat poslední knihu. V databázi musí zůstat alespoň jedna kniha!"); return false; }

            var smazani = MessageBox.Show("Opravdu chceš smazat knihu?", "Smazání knihy", MessageBoxButtons.YesNo);
            if (smazani == DialogResult.Yes)
            { Knihy.Remove(kniha); return true; }

            return false;
        }
        public static void VratitKnihy(Ctenar ctenar)//metoda pro vraceni vsech vypujcenych knih
        {
            foreach (var kniha in ctenar.Vypujcene.ToList())
            { kniha.Dostupnost = true; }
            ctenar.Vypujcene.Clear();
        }
        public static bool SmazatCtenare(Ctenar ctenar)//metoda pro smazani ctenare z databaze
        {
            if (Ctenari.Count == 1)
            { MessageBox.Show("Nemůžeš smazat posledního čtenáře. V databázi musí zůstat alespoň jeden čtenář!"); return false; }

            var smazani = MessageBox.Show("Opravdu chceš smazat čtenáře?", "Smazání čtenáře", MessageBoxButtons.YesNo);

            //jestli smazeme ctenare, ktery ma vypujcenou knihu, tak volame metodu VratitKnihy, ktera ji vrati
            if (smazani == DialogResult.Yes)
            { VratitKnihy(ctenar); Ctenari.Remove(ctenar); return true; }

            return false;
        }
        public static bool SmazatelnaKniha(Kniha kniha)
        {
            foreach (Ctenar ctenar in Ctenari)
            {
                if (ctenar.Vypujcene.Contains(kniha) || ctenar.Rezervovano.Contains(kniha))
                {
                    MessageBox.Show("Knihu nelze smazat, protože je vypůjčená nebo rezervovaná.");
                    return false;
                }
            }

            return true;
        }
        public static void Serializuj()//metoda ktera uklada data do XML souboru
        {
            SerializujList(Ctenari, "ctenari.xml");
            SerializujList(Knihy, "knihy.xml");
        }
        private static void SerializujList<T>(BindingList<T> list, string soubor)//metoda ktera seznamy do XML souboru
        {
            using (var s = File.Open(soubor, FileMode.Create))
            {
                var x = new XmlSerializer(typeof(BindingList<T>));
                x.Serialize(s, list);
            }
        }
        public static void Deserializuj()//metoda ktera nacita data ze souboru
        {
            Ctenari = DeserializujList<Ctenar>("ctenari.xml");
            Knihy = DeserializujList<Kniha>("knihy.xml");
            //aktualizuje odkazy na knihy podle ISBN
            foreach (var ctenar in Ctenari)//prochazi vsechny ctenare
            {
                //sjednotit vypujcene
                for (int i = 0; i < ctenar.Vypujcene.Count; i++)
                {
                    var k1 = ctenar.Vypujcene[i];
                    //najit knihu podle ISBN v Databaze.Knihy
                    var real = Knihy.FirstOrDefault(k => k.ISBN == k1.ISBN);
                    if (real != null)
                    { ctenar.Vypujcene[i] = real; }
                }
                //sjednotit Rezervovano
                for (int i = 0; i < ctenar.Rezervovano.Count; i++)
                {
                    var k1 = ctenar.Rezervovano[i];
                    var real = Knihy.FirstOrDefault(k => k.ISBN == k1.ISBN);
                    if (real != null)
                    { ctenar.Rezervovano[i] = real; }
                }
            }
        }
        private static BindingList<T> DeserializujList<T>(string soubor)//nacteni seznamu z XML souboru
        {
            if (!File.Exists(soubor)) return new BindingList<T>();
            using (var s = File.Open(soubor, FileMode.Open))
            {
                var x = new XmlSerializer(typeof(BindingList<T>));
                return (BindingList<T>)x.Deserialize(s);
            }
        }
    }
}
