namespace InvoiceLibrary
{
    public class DataServices
    {
        public object RecordId { get; set; }
        public object CaptionRecord { get; set; }
        public object Amount { get; set; }
        public object KindAmount { get; set; }
        public decimal AmountNetto { get; set; }
        public object VatRate { get; set; }
        public decimal ValueNetto { get; set; }
        public decimal ValueVat { get; set; }
        public decimal ValueBrutto { get; set; }
    }
}