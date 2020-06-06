using UnityEngine;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class FloatingSprite : MonoBehaviour
    {
        private Transform _currentTransform;

        public float Range = 0.1f;
        public float Velocity = 1f;

        private float _accumulator;

        private void Awake()
        {
            _currentTransform = transform;
        }

        private void Update()
        {
            _accumulator += Time.deltaTime * Velocity;
            if (_accumulator >= 360) _accumulator = 0;
            
            var position = _currentTransform.position;
            var newY = position.y + Mathf.Cos(_accumulator) * Time.deltaTime * Range;
            
            _currentTransform.position = new Vector3(position.x, newY, position.z);
        }
    }
}