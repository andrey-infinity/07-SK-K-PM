using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PacMan
{
    struct Record
    {
        public int Place;
        public Int64 Score;
        public string Name;
    }
    class SettingParser
    {
        private XmlDocument file = new XmlDocument();

        private string sNick = "";
        public string Nick
        {
            get { return sNick; }
            set
            {
                sNick = value;
                NickWrite(sNick);
            }
        }

        public Record GetRecord(int Place)
        {
            XmlNodeList records = file.GetElementsByTagName("best");
            Record result = new Record();
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Attributes[0].InnerText == Place.ToString())
                {
                    result.Place = Place;
                    result.Name = records[i].Attributes[1].InnerText;
                    result.Score = Convert.ToInt64(records[i].InnerText);
                }
            }
            return result;
        }
        public void SetRecord(int Place, string Nick, Int64 Score)
        {
            XmlNodeList records = file.GetElementsByTagName("best");
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Attributes[0].InnerText == Place.ToString())
                {
                    records[i].Attributes[1].InnerText = Nick;
                    records[i].InnerText = Score.ToString();
                }
            }
            file.Save("Settings.xml");
        }
        public SettingParser()
        {
            file.Load("Settings.xml");
            XmlNodeList nick = file.GetElementsByTagName("DefNick");
            sNick = nick[0].InnerText;
        }

        private void NickWrite(string value)
        {
            XmlNodeList nick = file.GetElementsByTagName("DefNick");
            nick[0].InnerText = value;
            file.Save("Settings.xml");
        }
    }
}
