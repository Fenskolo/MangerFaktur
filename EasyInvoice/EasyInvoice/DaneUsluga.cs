using System.Runtime.Serialization;

namespace EasyInvoice
{
    [DataContract]
    public class DaneUsluga
    {
        [DataMember]
        public int LpTabela { get; set; }
        [DataMember]
        public string OpisTabela { get; set; }
        [DataMember]
        public string Rodzajilosc { get; set; }
        [DataMember]
        public int Ilosc { get; set; }
        [DataMember]
        public decimal CenaNetto { get; set; }
        [DataMember]
        public decimal WartoscNetto { get; set; }
        [DataMember]
        public string StawkaVat { get; set; }
        [DataMember]
        public decimal KwotaVat { get; set; }
        [DataMember]
        public decimal WartoscBrutto { get; set; }
    }
}
