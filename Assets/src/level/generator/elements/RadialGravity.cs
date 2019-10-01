using System.Collections.Generic;
using System.Xml;
using src.level.generator.levels;

namespace src.level.generator.elements
{
    public class RadialGravity : Element
    {
        const float posRadius = 1.0f;

        public const float forceRadius = 4.0f;

        public float strength = 0;
        public float strengthRadius = 0;
        public bool invertAble = true;
        public bool disableAble = true;
        public List<ElementColor> colors = new List<ElementColor>();

        public RadialGravity(Position pos, float strength, float strengthRadius, List<ElementColor> colors) : base(ElementType.RadialGravity, posRadius)
        {
            positionX = pos.x;
            positionY = pos.y;
            this.strength = strength;
            this.strengthRadius = strengthRadius;
            this.colors = colors; 
        }

        public RadialGravity(Position pos, float strength, float strengthRadius, List<ElementColor> colors, int id) : 
                        base(ElementType.RadialGravity, posRadius, id)
        {
            positionX = pos.x;
            positionY = pos.y;
            this.strength = strength;
            this.strengthRadius = strengthRadius;
            this.colors = colors;
        }

        public bool containsColor(ElementColor color)
        {
            foreach (ElementColor c in colors)
            {
                if (c == color) return true;
            }

            return false;
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
            if (colors.Count > 1)
            {
                foreach (ElementColor color in colors)
                {
                    valColors += (valColors == "" ? "" : "|") + LevelHelper.colorNames[(int)color];
                }
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
