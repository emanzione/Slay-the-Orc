using UnityEngine;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class EnemyTierSetter : MonoBehaviour
    {
        public GameObject Common;
        public GameObject Uncommon;
        public GameObject Rare;
        public GameObject Legendary;
        
        public void SetTier(MonsterTier tier)
        {
            switch (tier)
            {
                case MonsterTier.Common:
                    Common.SetActive(true);
                    break;
                case MonsterTier.Uncommon:
                    Uncommon.SetActive(true);
                    break;
                case MonsterTier.Rare:
                    Rare.SetActive(true);
                    break;
                case MonsterTier.Legendary:
                    Legendary.SetActive(true);
                    break;
                default:
                    Common.SetActive(true);
                    break;
            }
        }
    }
}