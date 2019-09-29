using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levels
{
    public static class Randoms
    {
        public static Random rnd = null;

        public static void Init(int rndStart)
        { 
            rnd = new Random(rndStart);
        }

        public static int getInt(int maxValue)
        {
            return rnd.Next(maxValue);
        }

        public static int getInt(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue+1);
        }

        public static bool getProbality(int countValues)
        {
            int rndVal = rnd.Next(1, 1+Math.Max(1,countValues));
            return (rndVal == 1);
        }

        public static ElementColor getColor()
        {
            int first = (int)ElementColor._first_ + 1; ;
            int last = (int)ElementColor._last_ - 1;
            return (ElementColor) rnd.Next(first, last);
        }

        public static ElementColor getColor(List<ElementColor> excludedColors)
        {
            List<int> intColors = new List<int>();
            ElementColor selColor = 0;
            for (int i = (int)ElementColor._first_ + 1; i <= (int)ElementColor._last_ - 1; i++)
            {
                bool bAdd = true;
                foreach (ElementColor color in excludedColors)
                {
                    if ((int)color == i)
                    {
                        bAdd = false;
                        break;
                    }
                }
                if (bAdd)
                {
                    intColors.Add(i);
                }
            }

            if (intColors.Count > 0)
            {
                int idx = getInt(0, intColors.Count-1);
                selColor = (ElementColor) intColors[idx];
            }
            else
            {
                throw new Exception("getColor: no colour available");
            }

            return selColor;
        }

        public static int getSign()
        {
            int iSign = getInt(1, 2);
            if (iSign == 2) iSign = -1;
            return iSign;
        }

        public static float getAngleRad()
        {
            return (float)(rnd.NextDouble() * 2.0 * Math.PI);
        }

        public static Position getPosition(float radius)
        {
            float x = radius + (float) (rnd.NextDouble() * (LevelHelper.XDim - 2 * radius));
            float y = radius + (float) (rnd.NextDouble() * (LevelHelper.YDim - 2 * radius));
            return new Position(x,y); 
        }
        public static Position getPositionInCircle(Position posCenter, float radius)
        {
            float angle = getAngleRad();
            float factor = (float) rnd.NextDouble();
            float x = posCenter.x + (float) Math.Cos(angle) * factor * radius;
            float y = posCenter.y + (float)Math.Sin(angle) * factor * radius;
            return new Position(x, y);
        }
    }
}
