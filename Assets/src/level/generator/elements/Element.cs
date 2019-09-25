using System.Xml;
using src.level.generator.levels;

namespace src.level.generator.elements
{
    public abstract class Element
    {
        public static int idCounter = 0;

        public ElementType type = 0;
        public int id = 0;
        public float positionX = 0;
        public float positionY = 0;
        public float angle = 0;
        public float scaleX = 1;
        public float scaleY = 1;
        public float radius;

        public Element(ElementType type, float radius)
        {
            idCounter++;
            id = idCounter;
            this.type = type;
            this.radius = radius;
        }

        public Element(ElementType type, float radius, int id)
        {
            this.id = id;
            this.type = type;
            this.radius = radius;
        }
        public Position getPosition()
        {
            return new Position(positionX, positionY);
        }

        public abstract void WriteXML(XmlTextWriter writer);
        public void WriteBaseXML(XmlTextWriter writer)
        {
            writer.WriteStartElement("Position");
            writer.WriteAttributeString("x", positionX.ToString());
            writer.WriteAttributeString("y", positionY.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("Rotation");
            writer.WriteAttributeString("angle", angle.ToString());
            writer.WriteEndElement();

            if (scaleX != 1 || scaleY != 1)
            {
                writer.WriteStartElement("Scale");
                writer.WriteAttributeString("x", scaleX.ToString());
                writer.WriteAttributeString("y", scaleY.ToString());
                writer.WriteEndElement();
            }
        }

        public static int getLastID()
        {
            return idCounter;
        }
    }
}
