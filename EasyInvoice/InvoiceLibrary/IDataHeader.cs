using System.Collections.Generic;

namespace InvoiceLibrary
{
    internal interface IDataHeader
    {
        List<TableData> HeaderLeft { get; }
        List<TableData> HeaderRight { get; }
    }
}