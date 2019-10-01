using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace ManagerFaktur
{
    [Serializable()]
    public class Logs
    {
        private static Logs _log;
        private Logs() { }
        private MailSettings _ms;
        private DateTime _timeSent;
        private List<PairFiles> _fileOperation;

        public MailSettings Ms
        {
            get
            {
                if (_ms == null)
                {
                    _ms = MailSettings.Ins.MyInstance;
                }
                return _ms;
            }
            set => _ms = value;
        }

        public static Logs Log
        {
            get
            {
                if (_log == null)
                {
                    _log = new Logs();
                }
                return _log;
            }
        }

        public DateTime TimeSent { get => DateTime.Now; set => _timeSent = value; }

        [XmlIgnore]
        [Browsable(false)]
        public Logs MyInstance { get => _log; set => _log = value; }


        [Browsable(false)]
        public List<PairFiles> FileOperation
        {
            get
            {
                if (_fileOperation == null)
                {
                    _fileOperation = new List<PairFiles>();
                }
                return _fileOperation;
            }
            set => _fileOperation = value;
        }

        public static void SerializeXml()
        {
            TextWriter writer = null;

            try
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(Logs));
                writer = new StreamWriter(Settings.Instance.LogPath, true);
                xsSubmit.Serialize(writer, Log);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public void SerializeLog()
        {
            SerializeXml();
        }
    }

    [Serializable]
    public class PairFiles
    {
        [XmlElement(Order = 2)]
        public string News { get; set; }

        [XmlElement(Order = 1)]
        public string Old { get; set; }
    }
}
