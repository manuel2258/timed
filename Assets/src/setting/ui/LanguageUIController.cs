using System;
using System.Linq;
using src.translation;
using TMPro;

namespace src.setting.ui {
    public class LanguageUIController : BaseDropdownUIController<Language> {

        private Language[] _values;

        private void Start() {

            _values = Enum.GetValues(typeof(Language)).Cast<Language>().ToArray();
            
            foreach (Language value in _values) {
                dropdown.options.Add(new TMP_Dropdown.OptionData(value.ToString()));
            }
            
            dropdown.onValueChanged.AddListener(index => {
                setting.update(_values[index].ToString());
            });
            
            dropdown.SetValueWithoutNotify(_values.TakeWhile(language => language != setting.getValue()).Count());
            
        }
        
    }
}