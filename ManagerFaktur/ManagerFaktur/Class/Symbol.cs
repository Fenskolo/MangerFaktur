using System.ComponentModel;

namespace ManagerFaktur
{
    public class Symbol
    {
        [Category("Symbol")]
        [DisplayName("Start")]
        public string FirstString { get; set; }

        [Category("Symbol")]
        [DisplayName("End")]
        public string LastString { get; set; }

        [Category("Symbol")]
        [DisplayName("Typ")]
        public TypDanych Td { get; set; }
    }
}