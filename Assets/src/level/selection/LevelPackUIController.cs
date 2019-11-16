using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace src.level.selection {
    public class LevelPackUIController : MonoBehaviour {

        [SerializeField] private GameObject layoutPrefab;
        [SerializeField] private GameObject levelPreviewPrefab;

        [SerializeField] private TMP_Text packName;

        private List<LevelPack> _levelPacks;

        private int _selectionIndex;
        
        private void Start() {
            _levelPacks = LevelSelectionManager.Instance.LevelPacks;
            foreach (var levelPack in _levelPacks) {
                var newParent = Instantiate(layoutPrefab, transform);
                for (int i = 0; i < levelPack.LevelCount; i++) {
                    var newLevelPreview = Instantiate(levelPreviewPrefab, newParent.transform.GetChild(0).GetChild(0));
                    levelPack.loadLevelsFromAssets();
                    newLevelPreview.GetComponent<LevelPreviewUIController>().setup(levelPack[i]);
                }
                newParent.SetActive(false);
            }
            selectOtherPack(0);
        }

        public void selectOtherPack(int diff) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            _selectionIndex += diff;
            if (_selectionIndex > transform.childCount - 1) {
                _selectionIndex = 0;
            }

            if (_selectionIndex < 0) {
                _selectionIndex = transform.childCount - 1;
            }
            transform.GetChild(_selectionIndex).gameObject.SetActive(true);

            packName.text = _levelPacks[_selectionIndex].PackName;
        }
    }
}