using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.misc {
    public class IntToSpriteImageStorage : UnitySingleton<IntToSpriteImageStorage> {

        public Sprite one;
        public Sprite two;
        public Sprite three;
        public Sprite fore;
        public Sprite fife;
        public Sprite six;
        public Sprite seven;
        public Sprite eight;
        public Sprite nine;
        
        private readonly Dictionary<int, Sprite> _intSpriteMap = new Dictionary<int, Sprite>();

        private void Start() {
            _intSpriteMap.Add(1, one);
            _intSpriteMap.Add(2, two);
            _intSpriteMap.Add(3, three);
            _intSpriteMap.Add(4, fore);
            _intSpriteMap.Add(5, fife);
            _intSpriteMap.Add(6, six);
            _intSpriteMap.Add(7, seven);
            _intSpriteMap.Add(8, eight);
            _intSpriteMap.Add(9, nine);
        }

        public Sprite getSpriteByInt(int number) {
            if (!_intSpriteMap.TryGetValue(number, out var sprite)) {
                throw new Exception($"Could not find number {number} in IntToSpriteMap");
            }

            return sprite;
        }
    }
}