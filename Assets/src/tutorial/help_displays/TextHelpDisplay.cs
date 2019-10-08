using System;
using src.level.initializing;
using src.touch;
using src.tutorial.check_events;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.tutorial.help_displays {
    public class TextHelpDisplay : TouchableRect, ISetupAble, IPointerDownHandler, ICheckAbleEvent {

        public static TextHelpDisplay Instance;

        private const float MAX_SPEED = 15;

        private RectTransform _rectTransform;

        private Vector2 _lastPosition;
        private bool _hadLastFrame;

        private bool _transforming;
        private int _direction;

        private float _closedY;
        private float _openedY;
        
        public TMP_Text text;
        
        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }
        
        public void setup(string height, string content) {
            _rectTransform = transform as RectTransform;
            
            if (!float.TryParse(height, out var sizeY)) {
                throw new Exception("FrameHelpDisplay: Could not parse height argument -> " + height);
            }
            _rectTransform.sizeDelta = Vector2.zero;
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY);
            
            _openedY = 50 + sizeY / 2;
            _closedY = 50 - sizeY / 2;
            
            _rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, _openedY);
            
            _rectTransform.localScale = Vector3.one;

            text.text = content;
        }
        
        private void OnEnable() {
            Instance = this;
        }

        protected override void Update() {
            base.Update();
            if (hasPosition) {
                _checkEventManager.checkEvent("Touched");
                if (_hadLastFrame) {
                    var dif = _lastPosition - RectPosition;
                    if (Mathf.Abs(dif.y) > 15) {
                        _transforming = true;
                        _direction = -(int)Mathf.Sign(dif.y);
                    }
                }

                _lastPosition = RectPosition;
                _hadLastFrame = true;
            } else {
                _hadLastFrame = false;
            }

            if (_transforming) {
                _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition,
                    new Vector2(_rectTransform.anchoredPosition.x, _direction > 0 ? _openedY : _closedY), MAX_SPEED);
                if ((_rectTransform.anchoredPosition.y > _openedY - 1 && _rectTransform.anchoredPosition.y < _openedY + 1) ||
                    (_rectTransform.anchoredPosition.y > _closedY - 1 && _rectTransform.anchoredPosition.y < _closedY + 1)) {
                    _transforming = false;
                }
            }
        }
    }
}