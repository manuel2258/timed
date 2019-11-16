using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;

namespace src.misc {
    public class NeoSignController : MonoBehaviour {

        [SerializeField] private List<SpriteGlowEffect> signParts;

        [SerializeField] private float maxDelta;
        [SerializeField] private Vector2 timeRange;

        [SerializeField] private int timeoutChance;
        [SerializeField] private Vector2 blackoutTimeRange;
        [SerializeField] private Vector2 blackoutAmountRange;
        
        private float _normalBrightness;
        private float _timeToNextTarget;
        private float _deltaIntensity;

        private void Start() {
            _normalBrightness = signParts[0].GlowBrightness;
            foreach (var spriteGlowEffect in signParts) {
                spriteGlowEffect.GlowBrightness = _normalBrightness;
            }
            generateTarget();
        }
        
        private void generateTarget() {
            _timeToNextTarget = Random.Range(timeRange.x, timeRange.y);
            var targetBrightness = Random.Range(_normalBrightness - maxDelta, _normalBrightness + maxDelta);
            _deltaIntensity = (targetBrightness - signParts[0].GlowBrightness) / _timeToNextTarget;
        }

        private void Update() {
            foreach (var spriteGlowEffect in signParts) {
                spriteGlowEffect.GlowBrightness += _deltaIntensity * Time.deltaTime;
            }
            _timeToNextTarget -= Time.deltaTime;
            if (_timeToNextTarget <= 0) {
                generateTarget();
            }

            if (Random.Range(0, timeoutChance) == 0) {
                StartCoroutine(blackout((int)Random.Range(blackoutAmountRange.x, blackoutAmountRange.y)));
            }
        }

        private IEnumerator blackout(int amount) {
            for (int i = 0; i < amount; i++) {
                foreach (var spriteGlowEffect in signParts) {
                    spriteGlowEffect.AlphaThreshold = 0;
                }
                yield return new WaitForSeconds(Random.Range(blackoutTimeRange.x, blackoutTimeRange.y));
                foreach (var spriteGlowEffect in signParts) {
                    spriteGlowEffect.AlphaThreshold = 1;
                }
                yield return new WaitForSeconds(Random.Range(blackoutTimeRange.x, blackoutTimeRange.y));
            }
        }
    }
    
    
}