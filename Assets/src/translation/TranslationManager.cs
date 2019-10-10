using System.Collections.Generic;
using System.Linq;
using src.misc;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.translation {
    
    public class TranslationManager : UnitySingleton<TranslationManager> {
        
        private const string TRANSLATION_DIRECTORY = "translation_files/";
        
        private readonly Dictionary<string, WordContainer> _tagContainers = new Dictionary<string, WordContainer>();
        private List<TMP_Text> _texts = new List<TMP_Text>();

        private void Start() {
            loadTranslationWords();
            changeTextsToFitTranslation(Language.English);
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

        public void changeTextsToFitTranslation(Language language) {
            _texts.Clear();
            foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects()) {
                getAllChildTexts(rootGameObject.transform);
            }

            foreach (var tmpText in _texts) {
                var text = tmpText.text;
                if (text[0] != '[' || text[text.Length - 1] != ']') continue;

                var wordTag = text.Substring(1, text.Length - 2);
                tmpText.text = _tagContainers[wordTag].getTranslationByLanguage(language);
            }
        }

        private void getAllChildTexts(Transform parent) {
            for (int i = 0; i < parent.childCount; i++) {
                var currentChild = parent.GetChild(i);
                var text = currentChild.GetComponent<TMP_Text>();
                if (text != null) {
                    _texts.Add(text);
                }

                getAllChildTexts(currentChild);
            }
        }
        
    }
}