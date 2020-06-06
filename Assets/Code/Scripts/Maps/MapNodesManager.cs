using System;
using MHLab.InfectionsBlaster.Extensions;
using MHLab.SlayTheOrc.Maps.Nodes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MHLab.SlayTheOrc.Maps
{
    public sealed class MapNodesManager : MonoBehaviour
    {
        public static MapNode GeneratedMap;
        public static MapNodesManager Instance;
        
        public Transform StartingPoint;
        private Vector2 _startingPoint;

        public int NodesAmount;
        public float MinDistanceBetweenNodes;
        public float MaxDistanceBetweenNodes;
        
        private MapNode _path;
        private LineRenderer _lineRenderer;

        public MapNode EncounterNode;
        public MapNode PortalNode;
        public MapNode BossNode;
        public MapNode ChestNode;

        private void Awake()
        {
            _startingPoint = StartingPoint.transform.position;
            _lineRenderer = gameObject.GetComponentNoAlloc<LineRenderer>();
            _lineRenderer.positionCount = NodesAmount;

            if (GeneratedMap == null)
            {
                GeneratedMap = Generate();
                DontDestroyOnLoad(gameObject);
                DontDestroyOnLoad(this);
                Instance = this;
            }
        }

        private MapNode Generate()
        {
            var mapNode = GameObject.Instantiate(PortalNode, _startingPoint, Quaternion.identity);
            mapNode.transform.parent = transform;
            var firstNode = mapNode;
            var previousPosition = _startingPoint;
            var previousPositions = new Vector2[NodesAmount];
            previousPositions[0] = _startingPoint;
            
            _lineRenderer.SetPosition(0, _startingPoint);
            
            for (var i = 1; i < NodesAmount; i++)
            {
                var position = GeneratePosition(previousPosition, previousPositions, MaxDistanceBetweenNodes, MinDistanceBetweenNodes);

                previousPositions[i] = position;
                
                MapNode newNode = null;

                if (i == NodesAmount - 1)
                {
                    newNode = GameObject.Instantiate(BossNode, position, Quaternion.identity);
                }
                else
                {
                    if (Random.value <= 0.1f)
                    {
                        newNode = GameObject.Instantiate(ChestNode, position, Quaternion.identity);
                    }
                    else
                    {
                        newNode = GameObject.Instantiate(EncounterNode, position, Quaternion.identity);
                    }
                }
                
                newNode.transform.parent = transform;
                
                var lineRenderer = newNode.gameObject.AddComponent<LineRenderer>();
                lineRenderer.colorGradient = _lineRenderer.colorGradient;
                lineRenderer.material = _lineRenderer.material;
                lineRenderer.widthCurve = _lineRenderer.widthCurve;
                lineRenderer.widthMultiplier = _lineRenderer.widthMultiplier;
                
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, previousPosition + ((position - previousPosition).normalized) * 0.2f);
                lineRenderer.SetPosition(1, position + ((previousPosition - position).normalized) * 0.2f);
                
                mapNode.Next = newNode;
                newNode.Previous = mapNode;

                previousPosition = position;
                mapNode = newNode;
            }

            return firstNode;
        }

        private Vector2 GeneratePosition(Vector2 previousPosition, Vector2[] previousPositions, float maxDistance, float minDistance)
        {
            Vector2 newPosition = default;
            int attempts = 0;
            
            do
            {
                var distance = Random.Range(minDistance, maxDistance);

                var direction = Random.insideUnitCircle.normalized * (distance);

                newPosition = new Vector2(previousPosition.x + direction.x,
                    previousPosition.y + Mathf.Abs(direction.y));

                attempts++;
            } while (IsTooCloseToOtherNodes(newPosition, previousPositions, minDistance) && attempts <= 10);
            
            return newPosition;
        }

        private bool IsTooCloseToOtherNodes(Vector2 newPosition, Vector2[] otherPositions, float threshold)
        {
            for (var i = 0; i < otherPositions.Length; i++)
            {
                var other = otherPositions[i];
                if (Vector2.Distance(newPosition, other) <= threshold || Mathf.Abs(other.y - newPosition.y) <= threshold)
                {
                    return true;
                }
            }

            return false;
        }

        public static MapNode GetMapNode(int index)
        {
            if (index == 0) return GeneratedMap;

            var current = GeneratedMap.Next;

            for (var i = 1; current != null; i++)
            {
                if (index == i) return current;

                current = current.Next;
            }
            
            throw new IndexOutOfRangeException();
        }
    }
}