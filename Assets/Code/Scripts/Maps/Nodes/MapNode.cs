using UnityEngine;

namespace MHLab.SlayTheOrc.Maps.Nodes
{
    public abstract class MapNode : MonoBehaviour
    {
        public MapNode Next;
        public MapNode Previous;
        public MapNodeStatus Status;

        public Sprite SolvedSprite;
        protected SpriteRenderer Renderer;
        
        protected virtual void Awake()
        {
            Renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        }

        public void DisableRenderer()
        {
            Renderer.enabled = false;
        }

        public void EnableRenderer()
        {
            Renderer.enabled = true;
        }

        public virtual void MarkAsSolved()
        {
            Status = MapNodeStatus.Solved;
            Renderer.sprite = SolvedSprite;
        }

        public abstract void Activate();
    }
}