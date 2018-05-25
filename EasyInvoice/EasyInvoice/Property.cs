using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyInvoice
{
    public class Property
    {
        private static Property _instance;
        List<string> _nameList;
        List<string> _stawkaList;
        public Property() { }

        public static Property Instance
        {
            get
            {
                if(_instance ==null)
                {
                    _instance = new Property();
                }
                return _instance;
            }
            set => _instance = value;
        }

        public List<string> NameList
        {
            get
            {
                if (_nameList == null)
                {
                    _nameList = new List<string>()
                    {
                        "szt.",
                        "godz.",
                        "m2",
                        "op.",
                        "mc.",
                        "ton",
                        "km.",
                        "m3."
                    };
                }
                return _nameList;
            }
            
            set => _nameList = value;
        }

        public List<string> StawkaList
        {
            get
            {
                if (_stawkaList == null)
                {
                    _stawkaList = new List<string>()
                    {
                        "23%"
                    };
                }
                return _stawkaList;
            }

            set => _stawkaList = value;
        }
    }
}
