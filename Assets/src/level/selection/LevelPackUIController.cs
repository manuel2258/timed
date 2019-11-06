using System.Collections.Generic;
using Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.level.selection {
    public class LevelPackUIController : MonoBehaviour {

        [SerializeField] private GameObject layoutPrefab;
        [SerializeField] private GameObject levelButtonPrefab;

        [SerializeField] private TMP_Text packName;

        private List<LevelPack> _levelPacks;

        private int _selectionIndex;
        
        private void Start() {
            _levelPacks = LevelSelectionManager.Instance.LevelPacks;
            foreach (var levelPack in _levelPacks) {
                var newParent = Instantiate(layoutPrefab, transform);
                for (int i = 0; i < levelPack.LevelCount; i++) {
                    var newButton = Instantiate(levelButtonPrefab, newParent.transform);
                    var id = i;
                    var pack = levelPack;
                    newButton.GetComponent<Button>().onClick.AddListener(() => {
                        LevelSelectionManager.Instance.loadIndexFromPack(pack, id);
                    });
                    newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = id.ToString();
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