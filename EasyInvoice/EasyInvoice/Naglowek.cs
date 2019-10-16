using System;

namespace EasyInvoice
{
    public class Naglowek : ICloneable
    {
        public string NumerFaktury { get; set; }
        public string MiejsceWystawienia { get; set; }
        public DateTime DataWystawienia { get; set; }
        public DateTime DataSprzedazy { get; set; }
        public DateTime TerminZaplaty { get; set; }
        public string FormaPlatnosci { get; set; }
        public DateTime DataUtworzenia { get; set; }
        public int Id { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}