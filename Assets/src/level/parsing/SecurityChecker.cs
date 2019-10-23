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
        /// <exception cref="Exception">If the Hashes are not matching</exception>
        public static bool validateXmlLevel(XmlNode levelFile) {
            var haveHash = ParseHelper.getAttributeValueByName(levelFile, "levelHash");
            var sha256 = new SHA256Managed();
            byte[] levelXml = sha256.ComputeHash(Encoding.UTF8.GetBytes(levelFile.InnerXml));
            var shouldHash = Convert.ToBase64String(levelXml);
            if (shouldHash != haveHash) {
                throw new Exception($"Levelhash are not matching! Have: {haveHash} Should: {shouldHash}");
            }

            return true;
        }
    }
}