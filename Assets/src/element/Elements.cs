using System;
using src.element.effector;
using src.time.timeline;

namespace src.element {
    public static class Elements {
        public static void executeVisualChange(IVisualStateAble that, Action changeFunction) {
            var beforeState = that.getCurrentState();
            changeFunction.Invoke();
            var afterState = that.getCurrentState();
            ReplayTimeline.Instance.addVisualEvent(that, beforeState, afterState);
        }
    }
}