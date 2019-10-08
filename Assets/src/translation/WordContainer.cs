using System.Collections.Generic;

namespace src.translation {
    
    /// <summary>
    /// A container for several translation of word_tag
    /// </summary>
    public class WordContainer {

        public readonly string wordTag;
        private readonly Dictionary<Language, string> _translation = new Dictionary<Language, string>();

        public WordContainer(string tag) {
            wordTag = tag;
        }

        public void addWord(Language language, string word) {
            _translation.Add(language, word);
        }

        public string getTranslationByLanguage(Language language) {
            return _translation[language];
        }
    }
}