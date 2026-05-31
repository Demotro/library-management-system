using System;

namespace Knihovna
{
    [Serializable]
    public class Vypujcka
    {
        public int Id { get; set; }

        public int KnihaId { get; set; }

        public int CtenarId { get; set; }

        public DateTime DatumVypujceni { get; set; }

        public DateTime? DatumVraceni { get; set; }

        public string Stav { get; set; } = "Aktivni";

        public Vypujcka()
        {
        }

        public Vypujcka(int knihaId, int ctenarId)
        {
            KnihaId = knihaId;
            CtenarId = ctenarId;
            DatumVypujceni = DateTime.Now;
            Stav = "Aktivni";
        }
    }
}