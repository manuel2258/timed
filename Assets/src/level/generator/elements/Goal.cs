﻿using System.Xml;
using src.level.generator.levels;

namespace src.level.generator.elements
{
    public class Goal : Element
    {
        public const float posRadius = 1.0f;

        public int counter;
        public ElementColor color;

        public int counterTimed = 0;

        public Goal(Position pos, int counter, ElementColor color) : base(ElementType.Goal, posRadius)
        {
            positionX = pos.x;
            positionY = pos.y;
            this.counter = counter;
            this.color = color;
        }

        public override void WriteXML(XmlTextWriter writer)
        {
            writer.WriteStartElement(LevelWriter.TRIGGER);
            writer.WriteAttributeString(LevelWriter.TYPE, "Goal");
            //writer.WriteAttributeString("scriptName", "");
            writer.WriteAttributeString("id", id.ToString());

            base.WriteBaseXML(writer);

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "color");
            string valColor = LevelHelper.colorNames[(int)color];
            writer.WriteString(valColor);
            writer.WriteEndElement();

            writer.WriteStartElement(LevelWriter.PARAM);
            writer.WriteAttributeString(LevelWriter.NAME, "amount");
            writer.WriteString(counter.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

    }
}
