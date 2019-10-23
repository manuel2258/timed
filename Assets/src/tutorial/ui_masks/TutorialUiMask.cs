using System;
using src.level.initializing;
using src.misc;
using UnityEngine;

namespace src.tutorial.ui_masks {
    public class TutorialUiMask : MonoBehaviour, ISetupAble {
        
        public void setup(string positionX, string positionY, string sizeX, string sizeY, string anchorPosition) {
            var rectTransform = transform as RectTransform;
            
            var position = new Vector2(0, 0);
            if (!float.TryParse(positionX, out position.x)) {
                throw new Exception("FrameHelpDisplay: Could not parse positionX argument -> " + positionX);
            }
            
            if (!float.TryParse(positionY, out position.y)) {
                throw new Exception("FrameHelpDisplay: Could not parse positionY argument -> " + positionY);
            }
            
            var size = new Vector2(0, 0);
            if (!float.TryParse(sizeX, out size.x)) {
                throw new Exception("FrameHelpDisplay: Could not parse sizeX argument -> " + sizeX);
            }
            
            if (!float.TryParse(sizeY, out size.y)) {
                throw new Exception("FrameHelpDisplay: Could not parse sizeY argument -> " + sizeY);
            }

            if (!Enum.TryParse(anchorPosition, out AnchorPosition anchor)) {
                throw new Exception("FrameHelpDisplay: Could not parse anchor argument -> " + anchor);
            }

            var anchors = RectAnchorHelper.getAnchorByPosition(anchor);
            rectTransform.anchorMin = anchors.min;
            rectTransform.anchorMax = anchors.max;

            rectTransform.anchoredPosition = position;
            rectTransform.localScale = size;
        }
    }
}