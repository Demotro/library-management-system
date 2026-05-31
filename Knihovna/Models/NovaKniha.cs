using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knihovna
{
    public class NovaKniha : Kniha
    {
        //Konstruktor pro vytvořeni nove instance knihy
        public NovaKniha(string nazev, string autor, int isbn, int rokVydani)
            : base(nazev, autor, isbn, rokVydani)
        {
        }
        //konstruktor bez parametru, potrebny pro serializaci
        public NovaKniha() { }

        public override string StavKnihy => "Nový"; //vraci/prepisuje stav knihy
    }
}
