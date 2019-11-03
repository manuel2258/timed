using System;
using TMPro;
using UnityEngine;

namespace src.translation {
    
    [RequireComponent(typeof(TMP_Text))]
    public class TranslateAbleTMPText : MonoBehaviour {

        public string translationTag;
        public TMP_Text Text { get; private set; }

        public Action onTextChanged;

        private void Awake() {
            Text = GetComponent<TMP_Text>();
            TranslationManager.Instance.onLanguageChanged += translateText;
        }

        public void translateText() {
            try {
                Text.text = TranslationManager.Instance.getTranslatedStringByTag(translationTag);
            } catch (Exception) {
                Text.text = translationTag;
                Debug.Log($"Could not find translation for Tag: {translationTag}");
            }
            onTextChanged?.Invoke();
        }
        
    }        
}