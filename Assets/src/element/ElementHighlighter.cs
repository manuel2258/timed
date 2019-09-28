using System.Collections.ObjectModel;
using src.misc;
using UnityEngine;

namespace src.element {
    
    public class ElementHighlighter : UnitySingleton<ElementHighlighter> {

        [SerializeField] private GameObject highlighterPrefab;

        public void displayPositions(Collection<Vector2> positions) {
            deleteAllPositions();
            for (int i = 0; i < positions.Count; i++) {
                var newObject = Instantiate(highlighterPrefab, transform);
                newObject.transform.position = positions[i];
            }
        }

        public void deleteAllPositions() {
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

    }
}