using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasyInvoice
{
    [DataContract]
    public class Naglowek : ICloneable
    {
        [DataMember]
        public string NumerFaktury { get; set; }
        [DataMember]
        public string MiejsceWystawienia { get; set; }
        [DataMember]
        public DateTime DataWystawienia { get; set; }
        [DataMember]
        public DateTime DataSprzedazy { get; set; }
        [DataMember]
        public DateTime TerminZaplaty { get; set; }
        [DataMember]
        public string FormaPlatnosci { get; set; }
        [DataMember]
        public DateTime DataUtworzenia { get; set; }
        [DataMember]
        public int Id{ get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
