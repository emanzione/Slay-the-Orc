using MHLab.SlayTheOrc.Maps;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.SlayTheOrc
{
    public sealed class Player : MonoBehaviour
    {
        public Text GoldValue;
        public Text PotionsValue;
        public Text HealthValue;
        
        private Transform _playerTransform;

        public Camera MainCamera;
        private Transform _mainCameraTransform;

        public float Speed = 1.5f;
        private Vector3 _targetPosition;
        private bool _arrivedToTarget;
        
        public GameObject PressSpaceText;
        private int _pressedSpaceCounter;

        private void Awake()
        {
            _mainCameraTransform = MainCamera.transform;
            _playerTransform = transform;
        }

        private void Start()
        {
            var loot = LootManager.CurrentBag;
            GoldValue.text = loot.Gold.ToString();
            PotionsValue.text = loot.SmallPotions.ToString();
            HealthValue.text = PlayerManager.MaxHP.ToString();
            
            SetPositionInMap(PlayerManager.NodeIndexInMap);
        }

        private void Update()
        {
            if (!_arrivedToTarget)
            {
                var position = _playerTransform.position;
                var direction = (_targetPosition - position);

                if (direction.magnitude >= 0.1f)
                {
                    direction = direction.normalized;

                    var velocity = position + (direction * (Time.deltaTime * Speed));

                    _playerTransform.position = velocity;
                    _mainCameraTransform.position = new Vector3(velocity.x, velocity.y, _mainCameraTransform.position.z);
                }
                else
                {
                    _playerTransform.position = _targetPosition;
                    _mainCameraTransform.position = new Vector3(_targetPosition.x, _targetPosition.y, _mainCameraTransform.position.z);
                    _arrivedToTarget = true;

                    var currentNode = MapNodesManager.GetMapNode(PlayerManager.NodeIndexInMap);
                    currentNode.DisableRenderer();
                    
                    if (currentNode.Status != MapNodeStatus.Solved)
                        currentNode.Activate();
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    GoToNextPositionInMap();

                    if (_pressedSpaceCounter < 2)
                    {
                        _pressedSpaceCounter++;

                        if (_pressedSpaceCounter >= 2)
                        {
                            PressSpaceText.SetActive(false);
                        }
                    }
                }
            }
        }

        private void GoToNextPositionInMap()
        {
            var previousNode = MapNodesManager.GetMapNode(PlayerManager.NodeIndexInMap);
            previousNode.MarkAsSolved();
            previousNode.EnableRenderer();
            
            PlayerManager.NodeIndexInMap++;
            var currentNode = MapNodesManager.GetMapNode(PlayerManager.NodeIndexInMap);
            var currentNodePosition = currentNode.transform.position;
            _targetPosition = currentNodePosition;
            
            _arrivedToTarget = false;
        }
        
        private void SetPositionInMap(int nodeIndex)
        {
            var currentNode = MapNodesManager.GetMapNode(nodeIndex);
            currentNode.DisableRenderer();
            var currentNodePosition = currentNode.transform.position;
            
            _playerTransform.position = currentNodePosition;
            _mainCameraTransform.position = new Vector3(currentNodePosition.x, currentNodePosition.y, _mainCameraTransform.position.z);
            _targetPosition = currentNodePosition;
        }
    }
}