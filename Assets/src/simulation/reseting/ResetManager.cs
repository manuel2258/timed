using System.Collections.Generic;
using System.Linq;
using src.misc;
using UnityEngine;

namespace src.simulation.reseting {
    public class ResetManager : UnitySingleton<ResetManager> {
        
        private List<IResetable> _resetAbles = new List<IResetable>();
        
        private void Start() {
            var allGameObjects = FindObjectsOfType<GameObject>();
            _resetAbles = (from a in allGameObjects where a.GetComponent<IResetable>() != null 
                select a.GetComponent<IResetable>()).ToList();

            SimulationManager.Instance.onCalculationStarted += reset;
        }
        
        private void reset(bool wasSide) {
            foreach (var resetAble in _resetAbles) {
                resetAble.reset();
            }
        }
    }
}