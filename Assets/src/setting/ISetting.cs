using System;

namespace src.setting {
    public interface ISetting {
        string getValuesAsString();
        void addOnUpdatedCallback(Action callback);
    }
}