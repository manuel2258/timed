using UnityEngine;

namespace src.setting.ui {
    public abstract class ISettingsUIController : MonoBehaviour {
        public abstract void setup(string groupName);
    }
}