namespace src.level.parsing {
    
    /*public class XmlHashWindow : EditorWindow {
        private string _currentXmlPath;

        private string _displayedXmlPath;
        private bool _displayInfo;

        private string _currentHash;
        private string _newHash;
        private XmlDocument _currentXmlDocument;
        
        [MenuItem("Window/XmlHashWindow")]
        public static void ShowWindow() {
            GetWindow(typeof(XmlHashWindow));
        }

        void OnGUI() {
            _currentXmlPath = GUILayout.TextField(_currentXmlPath);
            var text = (TextAsset)Resources.Load(_currentXmlPath);
            if (text != null) {
                _displayedXmlPath = _currentXmlPath;
                _displayInfo = true;
                
                _currentXmlDocument = new XmlDocument();
                _currentXmlDocument.LoadXml(text.text);
                var xmlNode = _currentXmlDocument.SelectSingleNode("LevelFile");
                _newHash = SecurityChecker.getHashOfXmlNode(xmlNode);
                _currentHash = ParseHelper.getAttributeValueByName(xmlNode, "levelHash");
                xmlNode.Attributes["levelHash"].Value = _newHash;
            }

            if (_displayInfo) {
                GUILayout.Label(_currentHash);
                GUILayout.Label(_newHash);
                if (GUILayout.Button("Update Hash")) {
                    _currentXmlDocument.Save(Application.dataPath + "/Resources/" + _currentXmlPath + ".xml");
                }
            }

            if (_currentXmlPath != _displayedXmlPath) {
                _displayedXmlPath = "";
                _displayInfo = false;
            }
        }
    }*/
}