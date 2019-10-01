using System.Xml;
using src.level.generator.levels;

namespace src.level.generator.elements
{
    class ColliderBody : Element
    {
        public const float posRadius = 0.5f;

        public ElementColor color;

        public ColliderBody(Position pos, ElementColor color) : base(ElementType.ColliderBody, posRadius)
        {
            positionX = pos.x;
            positionY = pos.y;
            this.color = color;
        }

        public ColliderBody(Position pos, ElementColor color, int id) : base(ElementType.ColliderBody, posRadius, id)
        {
            positionX = pos.x;
            positionY = pos.y;
            this.color = color;
        }
        public override void WriteXML(XmlTextWriter writer)
        {
            writer.WriteStartElement("ColliderBody");
            writer.WriteAttributeString("id", id.ToString());

            base.WriteBaseXML(writer);
            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "initialColor");
            string valColor = LevelHelper.colorNames[(int)color];
            writer.WriteString(valColor);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
