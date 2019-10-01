using System.Collections.Generic;

namespace InvoiceLibrary
{
    public class InvoiceData
    {
        public string InvoiceNumber { get; set; }
        public string NrBankowyLeftRight { get; set; }
        public string ZapDoZapLeftRight { get; set; }
        public List<TableData> Summary { get; set; }
        public string SummaryLeftRight { get; set; }
        public string SummaryTextLeftRight { get; set; }
        public List<TableData> HeaderLeft { get; set; }
        public List<TableData> HeaderRight { get; set; }
        public List<TableData> GetSprzeNaby { get; set; }
        public List<TableData> NrBankowy { get; set; }
        public IEnumerable<DataServices> DataServices { get; set; }
        public IEnumerable<DataServices> SummaryServiceValues { get; set; }
        public List<TableData> ZapDoZap { get; set; }
        public List<TableData> SummaryText { get; set; }
    }
}