using System.Collections.Generic;
using System.Linq;
using src.simulation;
using UnityEngine;

namespace src.misc {
    public class PositionLineDrawer : MonoBehaviour {

        public GameObject linePrefab;

        public GameObject sideLinePrefab;

        public Transform lineParent;
        public Transform sideParent;

        private void Start() {
            SimulationManager.Instance.onCalculationFinished += (tracker, wasSide) => {
                if (!wasSide) {
                    drawNewPositions(tracker);
                    deleteSidePositions();
                } else {
                    drawNewSidePositions(tracker);
                }
            };
        }

        private void drawNewPositions(List<GameObjectTracker> trackers) {
            for (int i = 0; i < lineParent.childCount; i++) {
                Destroy(lineParent.GetChild(i).gameObject);
            }

            foreach (var gameObjectTracker in trackers) {
                var newObject = Instantiate(linePrefab, lineParent);
                var newLinRenderer = newObject.GetComponent<LineRenderer>();
                var positions = gameObjectTracker.Positions;
                newLinRenderer.positionCount = positions.Count;
                newLinRenderer.SetPositions(positions.Values.ToArray());
            }
        }
        
        private void drawNewSidePositions(List<GameObjectTracker> trackers) {
            deleteSidePositions();

            foreach (var gameObjectTracker in trackers) {
                var newObject = Instantiate(sideLinePrefab, sideParent);
                var newLinRenderer = newObject.GetComponent<LineRenderer>();
                var positions = gameObjectTracker.Positions;
                newLinRenderer.positionCount = positions.Count;
                newLinRenderer.SetPositions(positions.Values.ToArray());
            }
        }

        public void deleteSidePositions() {
            for (int i = 0; i < sideParent.childCount; i++) {
                Destroy(sideParent.GetChild(i).gameObject);
            }
        }
    }
}