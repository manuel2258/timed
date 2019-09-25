using src.misc;
using UnityEngine;

namespace src.level.finish {
    public class DifficultyUIController : UnitySingleton<DifficultyUIController> {

        public Transform starParent;

        public void displayDifficulty(int difficulty) {
            for (int i = 0; i < starParent.childCount-1; i++) {
                starParent.GetChild(i).gameObject.SetActive(false);
            }
            
            for (int i = 0; i < difficulty-1; i++) {
                starParent.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}