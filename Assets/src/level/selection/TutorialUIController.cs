using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src.level.selection {
    public class TutorialUIController : MonoBehaviour {

        public List<Sprite> tutorialPages;

        public Image display;

        private int _index;

        private void Start() {
            displayImageAtIndex();
        }

        private void displayImageAtIndex() {
            display.sprite = tutorialPages[_index];
        }

        public void addToIndex(int dif) {
            _index += dif;
            _index = Mathf.Clamp(_index, 0, tutorialPages.Count - 1);
            displayImageAtIndex();
        }
    }
}