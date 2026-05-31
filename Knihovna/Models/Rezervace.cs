using System;

namespace Knihovna
{
    [Serializable]
    public class Rezervace
    {
        public int Id { get; set; }

        public int KnihaId { get; set; }

        public int CtenarId { get; set; }

        public DateTime DatumRezervace { get; set; }

        public string Stav { get; set; } = "Aktivni";

        public Rezervace()
        {
        }

        public Rezervace(int knihaId, int ctenarId)
        {
            KnihaId = knihaId;
            CtenarId = ctenarId;
            DatumRezervace = DateTime.Now;
            Stav = "Aktivni";
        }
    }
}