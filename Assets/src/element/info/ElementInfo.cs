using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.element.info {
    
    /// <summary>
    /// A Scriptable Object that provides information about an effector and its events
    /// </summary>
    public class ElementInfo : ScriptableObject {
        
        public List<ElementEventInfo> elementEventInfos = new List<ElementEventInfo>();
        private readonly Dictionary<string, ElementEventInfo> _searchTagEventInfos = new Dictionary<string, ElementEventInfo>();

        public string elementName;
        public string helpText;

        public Sprite icon;

        private bool _ready;

        public ElementEventInfo getEventInfoBySearchTag(string searchTag) {
            if(!_ready) throw new Exception($"Tried to access the non initialized elementInfo {elementName}");
            
            if (!_searchTagEventInfos.TryGetValue(searchTag, out var eventInfo)) {
                throw new Exception($"Did not find SearchTag {searchTag} in {elementName}'s info");
            }
            return eventInfo;
        }

        public void buildInfos() {
            _ready = true;
            _searchTagEventInfos.Clear();
            elementEventInfos.ForEach(info => _searchTagEventInfos.Add(info.searchTag, info));
        }
    }
}