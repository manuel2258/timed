using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace src.level.parsing {
    public static class SecurityChecker {

        /// <summary>
        /// Checks whether the levels hashes are matching
        /// </summary>
        /// <param name="levelFile">The to check level</param>
        public static bool validateXmlLevel(XmlNode levelFile) {
            var haveHash = ParseHelper.getAttributeValueByName(levelFile, "levelHash");
            var shouldHash = getHashOfXmlNode(levelFile);
            return shouldHash == haveHash;
        }

        public static string getHashOfXmlNode(XmlNode xmlNode) {
            var sha256 = new SHA256Managed();
            var levelXml = sha256.ComputeHash(Encoding.UTF8.GetBytes(xmlNode.InnerXml));
            return Convert.ToBase64String(levelXml);
        }
    }
}