using System;
using System.Collections.Generic;
using src.misc;
using src.setting;
using UnityEngine;

namespace src.translation {
    
    public class TranslationManager : UnitySingleton<TranslationManager> {
        
        private const string TRANSLATION_DIRECTORY = "translation_files/";
        
        private readonly Dictionary<string, WordContainer> _tagContainers = new Dictionary<string, WordContainer>();

        private Language _currentLanguage = Language.German;

        
        public Language CurrentLanguage {
            get => _currentLanguage;
            set {
                _currentLanguage = value;
                onLanguageChanged?.Invoke();
            }
        }

        public Action onLanguageChanged;

        private void Awake() {
            loadTranslationWords();
        }

        private void Start() {
            SettingManager.Instance.GeneralSettings.Language.getValue(language => CurrentLanguage = language);
        }

        public void loadTranslationWords() {
            var assets = Resources.LoadAll<TextAsset>(TRANSLATION_DIRECTORY);
            foreach (var textAsset in assets) {
                var wordContainers = TranslationXmlParser.parseWordsFromXmlString(textAsset.text);
                foreach (var wordContainer in wordContainers) {
                    _tagContainers.Add(wordContainer.wordTag, wordContainer);
                }
            }
        }

        public string getTranslatedStringByTag(string translationTag) {
            return _tagContainers[translationTag].getTranslationByLanguage(_currentLanguage);
        }
        
    }
}