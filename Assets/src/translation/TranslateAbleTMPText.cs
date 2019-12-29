using System;
using TMPro;
using UnityEngine;

namespace src.translation {
    
    [RequireComponent(typeof(TMP_Text))]
    public class TranslateAbleTMPText : MonoBehaviour {

        public string translationTag;
        public TMP_Text Text { get; private set; }

        public Action onTextChanged;

        public bool translateOnStart;

        private void Awake() {
            Text = GetComponent<TMP_Text>();
            TranslationManager.Instance.onLanguageChanged += translateText;
        }

        private void Start() {
            if (translateOnStart) {
                translateText();
            }
        }

        public void translateText() {
            try {
                Text.text = TranslationManager.Instance.getTranslatedStringByTag(translationTag);
            } catch (Exception) {
                Text.text = translationTag;
                Debug.Log($"Could not find translation for Tag: {translationTag} in Object: {name}");
            }
            onTextChanged?.Invoke();
        }
        
    }        
}