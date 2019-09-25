using System.Collections.Generic;
using System.Xml;
using src.element;
using src.level.generator.levels;
using ElementType = src.level.generator.levels.ElementType;

namespace src.level.generator.elements
{
    public class RadialGravity : Element
    {
        const float posRadius = 1.0f;

        public float strength = 0;
        public float strengthRadius = 0;
        public bool invertAble = true;
        public bool disableAble = true;
        public List<ElementColor> colors;

        public RadialGravity(Position pos, float strength, float strengthRadius, List<ElementColor> colors) : base(ElementType.RadialGravity, posRadius)
        {
            positionX = pos.x;
            positionY = pos.y;
            this.strength = strength;
            this.strengthRadius = strengthRadius;
            this.colors = colors; 
        }

        public override void WriteXML(XmlTextWriter writer)
        {
            writer.WriteStartElement(LevelWriter.EFFECTOR);
            writer.WriteAttributeString(LevelWriter.TYPE, "RadialGravityEffector");
            writer.WriteAttributeString("id", id.ToString());

            base.WriteBaseXML(writer);

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "force");
            writer.WriteString(strength.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "invertAble");
            writer.WriteString(invertAble.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "disableAble");
            writer.WriteString(disableAble.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "colors");
            string valColors = "";
            foreach (ElementColor color in colors)
            {
                valColors += (valColors=="" ? "" : "|") + LevelHelper.colorNames[(int) color];
            }
            writer.WriteString(valColors);
            writer.WriteEndElement();

            if (colors.Count >= 1)
            {
                writer.WriteStartElement(LevelWriter.PARAM);
                writer.WriteAttributeString(LevelWriter.NAME, "initialColor");
                writer.WriteString(LevelHelper.colorNames[(int) colors[0]]);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
