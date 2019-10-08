using System;
using src.level;
using src.level.initializing;
using src.misc;
using UnityEngine;

namespace src.tutorial.help_displays {
    public class ElementHighlightHelpDisplay : MonoBehaviour, ISetupAble {

        public void setup(string sizeX, string sizeY, string elementId) {
            var size = new Vector2(0, 0);
            if (!float.TryParse(sizeX, out size.x)) {
                throw new Exception("ElementHighlightHelpDisplay: Could not parse sizeX argument -> " + sizeX);
            }
            
            if (!float.TryParse(sizeY, out size.y)) {
                throw new Exception("ElementHighlightHelpDisplay: Could not parse sizeY argument -> " + sizeY);
            }

            if (!int.TryParse(elementId, out var id)) {
                throw new Exception("ElementHighlightHelpDisplay: Could not parse elementId argument -> " + elementId);
            }

            var toHighlight = LevelManager.Instance.CurrentLevel.getElementFromId(id);

            transform.position = toHighlight.transform.position;
            transform.localScale = size;
        }
    }
}