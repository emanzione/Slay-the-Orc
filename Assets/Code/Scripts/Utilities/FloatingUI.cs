using MHLab.InfectionsBlaster.Extensions;
using UnityEngine;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class FloatingUI : MonoBehaviour
    {
        private RectTransform _currentTransform;

        public float Range = 0.1f;
        public float Velocity = 1f;

        private float _accumulator;

        private void Awake()
        {
            _currentTransform = gameObject.GetComponentNoAlloc<RectTransform>();
        }

        private void Update()
        {
            _accumulator += Time.deltaTime * Velocity;
            if (_accumulator >= 360) _accumulator = 0;
            
            var position = _currentTransform.anchoredPosition3D;
            var newY = position.y + Mathf.Cos(_accumulator) * Time.deltaTime * Range;
            
            _currentTransform.anchoredPosition3D = new Vector3(position.x, newY, position.z);
        }
    }
}