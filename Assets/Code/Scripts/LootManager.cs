using UnityEngine;

namespace MHLab.SlayTheOrc
{
    public sealed class LootInfo
    {
        public int Gold;
        public int SmallPotions;
    }
    
    public static class LootManager
    {
        public static LootInfo CurrentBag = Initialize();

        private static LootInfo Initialize()
        {
            return new LootInfo()
            {
                Gold = Random.Range(6, 22),
                SmallPotions = InitializePotions()
            };
        }

        private static int InitializePotions()
        {
            var random = Random.value;
            if (random >= 0.8f)
            {
                return 1;
            }

            return 0;
        }
        
        public static LootInfo GetLootForMonster(MonsterInfo monster)
        {
            return new LootInfo()
            {
                Gold = GetGoldForMonster(monster),
                SmallPotions = InitializePotions()
            };
        }

        private static int GetGoldForMonster(in MonsterInfo monster)
        {
            switch (monster.Tier)
            {
                case MonsterTier.Common:
                    return Random.Range(5, 12);
                case MonsterTier.Uncommon:
                    return Random.Range(11, 18);
                case MonsterTier.Rare:
                    return Random.Range(17, 27);
                case MonsterTier.Legendary:
                    return Random.Range(27, 37);
                default:
                    return 5;
            }
        }

        public static void AddLootToBag(in LootInfo loot)
        {
            CurrentBag.Gold += loot.Gold;
            CurrentBag.SmallPotions += loot.SmallPotions;
        }
    }
}