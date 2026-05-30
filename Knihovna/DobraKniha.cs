using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knihovna
{
    public class DobraKniha : Kniha
    {
        //Konstruktor pro vytvořeni nove instance knihy
        public DobraKniha(string nazev, string autor, int isbn, int rokVydani)
            : base(nazev, autor, isbn, rokVydani)
        {
        }
        //konstruktor bez parametru, potrebny pro serializaci
        public DobraKniha() { }

        public override string StavKnihy => "Dobrý"; //vraci/prepisuje stav knihy
    }
}
