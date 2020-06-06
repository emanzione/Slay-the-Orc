using System;
using MHLab.InfectionsBlaster.Extensions;
using UnityEngine;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class Tooltip : MonoBehaviour
    {
        private const float MaxFloatingDistanceOnY = 0.5f;
        private const float Lifetime = 1f;

        public bool HasNoLifetime;
        
        private TextMesh _text;
        private Vector3 _initialPosition;
        private Vector3 _targetPosition;

        private float _lifeTimer;
        private bool _activated;

        private void Awake()
        {
            gameObject.SetActive(false);
            _initialPosition = transform.position;
        }

        private void OnEnable()
        {
            _text = gameObject.GetComponentInChildren<TextMesh>();
            _activated = false;
            _lifeTimer = 0;

            _targetPosition = new Vector3(_initialPosition.x, _initialPosition.y + MaxFloatingDistanceOnY, _initialPosition.z);
        }

        public void Activate(int value)
        {
            gameObject.SetActive(true);
            _text.text = value.ToString();
            transform.position = _initialPosition;
            _activated = true;
            _lifeTimer = 0;
        }

        public void Deactivate()
        {
            _activated = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (HasNoLifetime) return;
            
            if (_activated)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPosition, 1f * Time.deltaTime);
                _lifeTimer += Time.deltaTime;
            }

            if (_lifeTimer >= 1f)
            {
                Deactivate();
            }
        }
    }
}