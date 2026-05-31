using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knihovna
{
    public class OpotrebovanaKniha : Kniha
    { 
        //Konstruktor pro vytvořeni nove instance knihy
        public OpotrebovanaKniha(string nazev, string autor, int isbn, int rokVydani)
            : base(nazev, autor, isbn, rokVydani)
        {
        }
        //konstruktor bez parametru, potrebny pro serializaci
        public OpotrebovanaKniha() { }

        public override string StavKnihy => "Opotřebovaný"; //vraci/prepisuje stav knihy
    }
}
