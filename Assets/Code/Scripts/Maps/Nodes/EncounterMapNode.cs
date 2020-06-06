using UnityEngine.SceneManagement;

namespace MHLab.SlayTheOrc.Maps.Nodes
{
    public sealed class EncounterMapNode : MapNode
    {
        public override void Activate()
        {
            var mapNodesManager = FindObjectOfType<MapNodesManager>();
            mapNodesManager.gameObject.SetActive(false);
            SceneManager.LoadScene("Battle");
        }
    }
}