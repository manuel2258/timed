using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levels
{
    public enum ElementType
    {
        ColliderBody = 1,
        Wall = 2,
        Door = 3,
        Teleporter = 4,

        Goal = 11,
        Switch = 12,

        LinearGravity = 21,
        ColorChanger = 22,
        DestroyLaser = 23,
        RadialGravity = 24
    }

    public enum ElementColor
    {
        _first_ = 0,
        white = 1,
        green = 2,
        blue = 3,
        yellow = 4,
        _last_ = 5
    }

    public class Position
    {
        public float x;
        public float y;

        public Position(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class TimeLineEvent
    {
        public int idSource;
        public int idTarget;

        public TimeLineEvent(int idSource, int idTarget)
        {
            this.idSource = idSource;
            this.idTarget = idTarget;
        }
    }

    public static class GravityForce
    {
        public const float maxRadius = 7.0f;
        public const float constForceLow = 1500f;    // radius <= 4
        public const float constForceNormal = 2000f; // radius <= 6
        public const float constForceHigh = 2500f;   // radius <= 8

        public static float[] radiusToForce = { constForceLow, constForceLow, constForceLow, constForceLow,
                            constForceNormal, constForceNormal, constForceHigh, constForceHigh};

        public static float getForce(float radius)
        {
            int iRadius = (int) Math.Min(radius, maxRadius);
            return radiusToForce[iRadius];
        }

        public static float getForceRadius(float strength)
        {
            float radius = 0f;
               
            foreach (float str in radiusToForce)
            {
                if (str > strength) break;
                radius += 1.0f;
            }

            return radius;
        }
    }
    public static class LevelHelper
    {
        public const int XDim = 36;
        public const int YDim = 20;
        public const int maxDifficulty = 9;

        public static string[] colorNames = new string[] { "", "White", "Green", "Blue", "Yellow", "" };

        public static float getDistance(Position pos1, Position pos2)
        {
            return (float) Math.Sqrt((pos1.x-pos2.x)* (pos1.x - pos2.x) + (pos1.y - pos2.y)*(pos1.y - pos2.y));
        }

        public static Position getCenter(Position pos1, Position pos2)
        {
            return new Position((pos1.x + pos2.x) * 0.5f, (pos1.y + pos2.y) * 0.5f);
        }

    }
}
