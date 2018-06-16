using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasyInvoice
{
    [DataContract]
    public class FirmaData : ICloneable
    {
        [DataMember]
        public string NazwaFirmy { get; set; }
        [DataMember]
        public string UlicaFirmy { get; set; }
        [DataMember]
        public string MiastoFirmy { get; set; }
        [DataMember]
        public string NipFirmy { get; set; }
        [DataMember]
        public string InneFirmy { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
