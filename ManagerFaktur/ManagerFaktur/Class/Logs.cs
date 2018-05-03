using System;
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
            set
            {
                _ms = value;
            }
        }

        public static Logs Log
        {
            get
            {
                if(_log==null)
                {
                    _log = new Logs();
                    SerializeXml();
                }                
                return _log;
            }           
        }

        public DateTime TimeSent
        {
            get
            {               
                return DateTime.Now;
            }
            set
            {
                _timeSent = value;
            }            
        }

        [XmlIgnore]
        [Browsable(false)]
        public Logs MyInstance
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
            }
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
    }
}
