using System;
using UnityEngine;

namespace src.misc {
    public static class RectAnchorHelper {
        
        public static Anchor getAnchorByPosition(AnchorPosition anchorPosition) {
            switch (anchorPosition) {
                case AnchorPosition.Left:
                    return new Anchor {min = new Vector2(0, 0.5f), max = new Vector2(0, 0.5f)};
                case AnchorPosition.Right:
                    return new Anchor {min = new Vector2(1, 0.5f), max = new Vector2(1, 0.5f)};
                default:
                    throw new Exception("Could not parse AnchorPosition");
            }
        }
    }

    public struct Anchor {
        public Vector2 min;
        public Vector2 max;
    }

    public enum AnchorPosition {
        Left,
        Right
    }
}