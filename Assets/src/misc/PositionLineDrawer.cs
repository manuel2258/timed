using System.Collections.Generic;
using System.Linq;
using src.simulation;
using UnityEngine;

namespace src.misc {
    public class PositionLineDrawer : MonoBehaviour {

        public GameObject linePrefab;

        private void Start() {
            SimulationManager.Instance.onCalculationFinished += drawNewPositions;
        }

        private void drawNewPositions(List<GameObjectTracker> trackers) {
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }

            foreach (var gameObjectTracker in trackers) {
                var newObject = Instantiate(linePrefab, transform);
                var newLinRenderer = newObject.GetComponent<LineRenderer>();
                var positions = gameObjectTracker.Positions;
                newLinRenderer.positionCount = positions.Count;
                newLinRenderer.SetPositions(positions.Values.ToArray());
            }
        }
    }
}