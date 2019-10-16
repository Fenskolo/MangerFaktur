using System;

namespace EasyInvoice
{
    public class FirmaData : ICloneable
    {
        public string NazwaFirmy { get; set; }
        public string UlicaFirmy { get; set; }
        public string MiastoFirmy { get; set; }
        public string NipFirmy { get; set; }
        public string InneFirmy { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}