using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Knihovna
{
    [Serializable(), XmlInclude(typeof(NovaKniha)),
        XmlInclude(typeof(DobraKniha)), XmlInclude(typeof(OpotrebovanaKniha))]
    public abstract class Kniha
    {
        //privatni atributy pro ulozeni dat knihy
        private int _id;
        private string _nazev = string.Empty;
        private string _autor = string.Empty;
        private string _isbn = string.Empty;
        private int _rokVydani;
        private bool _dostupnost = true;

        //vlastnosti, ktere ctou a zapisuji privatni atributy
        public int Id
        { get => _id; set => _id = value; }

        public string Nazev
        { get => _nazev; set => _nazev = value; }

        public string Autor
        { get => _autor; set => _autor = value; }

        public string ISBN
        { get => _isbn; set => _isbn = value; }

        public int RokVydani
        { get => _rokVydani; set => _rokVydani = value; }

        public bool Dostupnost
        { get => _dostupnost; set => _dostupnost = value; }

        //abstraktni vlastnost, definuji ji tridy dedici z teto tridy
        public abstract string StavKnihy { get; }

        //konstruktor prazdne instance knihy, potrebny pro serializaci
        protected Kniha() { }

        //konstruktor pro vytvoreni instance knihy
        protected Kniha(string nazev, string autor, string isbn, int rokVydani)
        {
            Nazev = nazev;
            Autor = autor;
            ISBN = isbn;
            RokVydani = rokVydani;
        }
    }
}