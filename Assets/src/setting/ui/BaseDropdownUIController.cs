using TMPro;
using UnityEngine;

namespace src.setting.ui {
    public abstract class BaseDropdownUIController<TSettingsType> : BaseUIController<TSettingsType> {
        [SerializeField] protected TMP_Dropdown dropdown;
    }
}