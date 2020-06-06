using UnityEngine;

namespace MHLab.SlayTheOrc.Maps.Nodes
{
    public sealed class PortalMapNode : MapNode
    {
        public override void MarkAsSolved()
        {
            Status = MapNodeStatus.Solved;
        }
        
        public override void Activate()
        {
            Debug.Log("Portal node reached!");
        }
    }
}