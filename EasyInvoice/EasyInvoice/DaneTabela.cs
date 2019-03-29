using System.Runtime.Serialization;

namespace EasyInvoice
{
    [DataContract]
    public class DaneTabela
    {
        [DataMember]
        public string Lewa { get; set; }
        [DataMember]
        public string Prawa { get; set; }
    }
}
