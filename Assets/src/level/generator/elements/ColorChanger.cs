using System.Collections.Generic;
using System.Xml;
using src.element;
using src.level.generator.levels;
using ElementType = src.level.generator.levels.ElementType;

namespace src.level.generator.elements
{
    public class ColorChanger : Element
    {

        const float posRadius = 1.0f;

        public float size = 0;
        public ElementColor initialColor;
        public List<ElementColor> colors;

        public ColorChanger(Position pos, ElementColor color, float size = 0) : base(ElementType.ColorChanger, posRadius)
        {
            positionX = pos.x;
            positionY = pos.y;
            this.initialColor = color;
            colors = new List<ElementColor>();
        }

        public void addColor(ElementColor color)
        {
            colors.Add(color);
        }


        public override void WriteXML(XmlTextWriter writer)
        {
            writer.WriteStartElement(LevelWriter.EFFECTOR);
            writer.WriteAttributeString(LevelWriter.TYPE, "ColorChangerEffector");
            writer.WriteAttributeString("id", id.ToString());

            base.WriteBaseXML(writer);

            /*
            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "size");
            writer.WriteString(size.ToString());
            writer.WriteEndElement();
            */

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "colors");
            string valColors = "";
            foreach (ElementColor color in colors)
            {
                valColors += (valColors == "" ? "" : "|") + LevelHelper.colorNames[(int)color];
            }

            writer.WriteString(valColors);
            writer.WriteEndElement();

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "initialColor");
            writer.WriteString(LevelHelper.colorNames[(int)initialColor]);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
