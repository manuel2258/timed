using System;
using System.Collections.Generic;
using src.element.effector.effectors;
using src.misc;
using src.simulation.reseting;
using src.time.time_managers;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton representing a series of VisualEvents
    /// </summary>
    public class ReplayTimeline : UnitySingleton<ReplayTimeline>, IResetable {

        private readonly List<TimedVisualEvent> _visualEvents = new List<TimedVisualEvent>();

        private void Start() {
            ReplayTimeManager.Instance.onNewTime += onNewReplayTime;
        }

        public void addVisualEvent(IVisualStateAble stateChanger, VisualState before, VisualState after) {
            _visualEvents.Add(new TimedVisualEvent(SimulationTimeManager.Instance.CurrentTime, stateChanger, before, after));
        }

        private void onNewReplayTime(decimal currentTime, decimal deltaTime) {
            _visualEvents.Sort((x, y) => Math.Sign(x.ExecutionTime - y.ExecutionTime));
            if (deltaTime < 0) {
                _visualEvents.Reverse();
            }
            foreach (var timedEffectorEvent in _visualEvents) {
                if (deltaTime >= 0) {
                    if (timedEffectorEvent.ExecutionTime <= currentTime &&
                        timedEffectorEvent.ExecutionTime > currentTime - deltaTime) {
                        timedEffectorEvent.executeForward();
                    }
                } else {
                    if (timedEffectorEvent.ExecutionTime > currentTime &&
                        timedEffectorEvent.ExecutionTime < currentTime - deltaTime) {
                        timedEffectorEvent.executeBackwards();
                    }
                }
            }
        }

        public void reset() {
            _visualEvents.Clear();
        }
    }
}