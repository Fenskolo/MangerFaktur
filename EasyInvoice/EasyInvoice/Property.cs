using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable]
    public class Property
    {
        private static Property _mInstance;
        private static readonly string xmlPath = "xmlProperty.xml";
        private List<string> _mNameList;
        private List<string> _mStawkaList;
        private List<WorkClass> _mWorks;

        public static Property Instance
        {
            get => _mInstance ?? (_mInstance = File.Exists(xmlPath) ? Deserialize() : new Property());
            set => _mInstance = value;
        }

        public List<string> NameList
        {
            get
            {
                if (_mNameList == null || _mNameList.Count == 0)
                    _mNameList = new List<string>
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
                return _mNameList;
            }
            set
            {
                value.Where(f => NameList.Contains(f)).ToList().ForEach(a => value.Remove(a));
                _mNameList = value;
            }
        }

        public List<string> StawkaList
        {
            get
            {
                if (_mStawkaList == null || _mStawkaList.Count == 0)
                    _mStawkaList = new List<string>
                    {
                        "23%",
                        "0%",
                        "7%"
                    };
                return _mStawkaList;
            }
            set
            {
                value.Where(f => StawkaList.Contains(f)).ToList().ForEach(a => value.Remove(a));
                _mStawkaList = value;
            }
        }

        public List<WorkClass> Works
        {
            get => _mWorks ?? (_mWorks = new List<WorkClass>());

            set => _mWorks = value;
        }

        public static Property Deserialize()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Property));
                reader = new StreamReader(xmlPath);
                return (Property) serializer.Deserialize(reader);
            }
            finally
            {
                reader?.Close();
            }
        }

        public static void SerializeXml()
        {
            TextWriter writer = null;
            try
            {
                var xsSubmit = new XmlSerializer(typeof(Property));
                writer = new StreamWriter(xmlPath, false);
                xsSubmit.Serialize(writer, Instance);
            }
            finally
            {
                writer?.Close();
            }
        }
    }
}