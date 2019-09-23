using UnityEngine;

namespace src.element {
    public enum ElementColor {
        Yellow,
        Blue,
        Green,
        Red, 
    }

    /// <summary>
    /// Helper class to decode the ElementColor enum into a UnityColor
    /// </summary>
    public static class ElementColors {
        public static Color32 getColorValue(ElementColor color) {
            switch (color) {
                case ElementColor.Yellow:
                    return new Color32(255, 255, 0, 255);
                case ElementColor.Blue:
                    return new Color32(0, 255, 255, 255);
                case ElementColor.Green:
                    return new Color32(0, 255, 0, 255);
                case ElementColor.Red:
                    return new Color32(255,0,0, 255);
                default:
                    return Color.black;
            }
        }
    }
}