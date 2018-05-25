using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyInvoice
{
    class SingleFakturaProperty
    {
        private DataTable _dt;

        public DataTable Dt
        {
            get
            {
                if(_dt == null)
                {
                    _dt = new DataTable();
                    _dt.Columns.Add("Towar / usługa", typeof(string));
                    _dt.Columns.Add("J.m.");
                    _dt.Columns.Add("Ilość", typeof(Int32));
                    _dt.Columns.Add("Cena Netto", typeof(double));
                    _dt.Columns.Add("Wartość netto", typeof(double));
                    _dt.Columns.Add("Stawka VAT");
                    _dt.Columns.Add("Kwota VAT", typeof(double));
                    _dt.Columns.Add("Wartość Brutto", typeof(double));
                }

                return _dt;
            }
            
            set => _dt = value;
        }
    }
}
