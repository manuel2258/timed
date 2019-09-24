using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using elements;


namespace levels
{
    public class LevelWriter
    {
        public const string PARAM = "Parameter";
        public const string NAME = "name";
        public const string TYPE = "type";
        public const string TRIGGER = "Trigger";
        public const string EFFECTOR = "Effector";

        public string writeLevel(int level, int difficulty, List<Element> levelElements)
        {
            string xml = "";
            string name = string.Format("Generated Level {0},{1}",level,difficulty);
            StringBuilder builder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(builder))
            using (XmlTextWriter writer = new XmlTextWriter(stringWriter))
            { 
                writer.WriteStartDocument();
                //writer.Formatting = Formatting.Indented;
                //writer.Indentation = 4;

                writer.WriteStartElement("Level");
                writer.WriteAttributeString("name", name);
                writer.WriteAttributeString("id", level.ToString());
                writer.WriteAttributeString("difficulty", difficulty.ToString());

                writer.WriteStartElement("Elements");

                foreach (Element elem in levelElements)
                {
                    elem.WriteXML(writer);
//                    writer.Flush();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            return builder.ToString();
        }

        public void writeLevelToFile(string pathGame, int level, int difficulty, List<Element> levelElements)
        {
            string fileName = string.Format("{0:000000}_{1}.xml",level,difficulty);
            string path = Path.Combine(pathGame, fileName);

            string xml = writeLevel(level, difficulty, levelElements);

            File.WriteAllText(path, xml);
        }
        public static void writeLevelToFile(string pathGame, string xml, int level, int difficulty)
        {
            string fileName = string.Format("{0:000000}_{1}.xml", level, difficulty);
            string path = Path.Combine(pathGame, fileName);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.Save(path);


//            File.WriteAllText(path, xml);
        }
    }
}
