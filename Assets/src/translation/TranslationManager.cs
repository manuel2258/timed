using System.Collections.Generic;
using System.Linq;
using src.misc;
using TMPro;
using UnityEngine;

namespace src.translation {
    
    public class TranslationManager : UnitySingleton<TranslationManager> {
        
        private const string TRANSLATION_DIRECTORY = "translation_files/";
        
        private readonly Dictionary<string, WordContainer> _tagContainers = new Dictionary<string, WordContainer>();
        
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
            var allGameObjects = FindObjectsOfType<GameObject>();
            var texts = (from a in allGameObjects where a.GetComponent<TMP_Text>() != null 
                select a.GetComponent<TMP_Text>()).ToList();

            foreach (var tmpText in texts) {
                var text = tmpText.text;
                if (text[0] != '[' || text[text.Length - 1] != ']') continue;

                var wordTag = text.Substring(1, text.Length - 2);
                tmpText.text = _tagContainers[wordTag].getTranslationByLanguage(language);
            }
        }
        
    }
}