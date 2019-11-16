using System;
using src.level.initializing;
using src.touch;
using src.translation;
using src.tutorial.check_events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.tutorial.help_displays {
    public class TextHelpDisplay : TouchableRect, ISetupAble, IPointerDownHandler, ICheckAbleEvent {

        private const float MAX_SPEED = 100;

        private RectTransform _rectTransform;

        private Vector2 _lastPosition;
        private bool _hadLastFrame;

        private bool _transforming;
        private int _direction;

        private float _closedY;
        private float _openedY;
        
        public TranslateAbleTMPText translateAbleText;
        
        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }
        
        public void setup(string content) {
            _rectTransform = transform as RectTransform;
            _rectTransform.sizeDelta = Vector2.zero;
            _rectTransform.localScale = Vector3.one;

            translateAbleText.onTextChanged += scaleText;
            translateAbleText.translationTag = content;
            translateAbleText.translateText();
        }

        private void scaleText() {
            var text = translateAbleText.Text;
            var prefSize = text.GetPreferredValues();
            prefSize.y += 50;
            _openedY = 50 + prefSize.y / 2;
            _closedY = 50 - prefSize.y / 2;
            _rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, _openedY);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, prefSize.y);
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
                    new Vector2(_rectTransform.anchoredPosition.x, _direction > 0 ? _openedY : _closedY), MAX_SPEED * Time.deltaTime);
                if ((_rectTransform.anchoredPosition.y > _openedY - 1 && _rectTransform.anchoredPosition.y < _openedY + 1) ||
                    (_rectTransform.anchoredPosition.y > _closedY - 1 && _rectTransform.anchoredPosition.y < _closedY + 1)) {
                    _transforming = false;
                }
            }
        }
    }
}