using UnityEngine;

namespace MHLab.SlayTheOrc
{
    public sealed class MonsterInfo
    {
        public string Name;
        public int MaxHP;
        public MonsterTier Tier;
    }

    public enum MonsterTier
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
        Boss
    }
    
    public static class MonsterManager
    {
        public static MonsterInfo LastGeneratedMonster;
        
        public static MonsterInfo GenerateMonsterInfo()
        {
            var monster = new MonsterInfo();
            monster.Name = GenerateMonsterName();
            GenerateTierAndHP(out monster.MaxHP, out monster.Tier);

            LastGeneratedMonster = monster;

            return monster;
        }

        private static string GenerateMonsterName()
        {
            return "Monster";
        }

        private static void GenerateTierAndHP(out int hp, out MonsterTier tier)
        {
            var tierRandom = Random.value;

            if (tierRandom <= 0.5f)
            {
                tier = MonsterTier.Common;
            }
            else if (tierRandom <= 0.8f)
            {
                tier = MonsterTier.Uncommon;
            }
            else if (tierRandom <= 0.95f)
            {
                tier = MonsterTier.Rare;
            }
            else
            {
                tier = MonsterTier.Legendary;
            }

            switch (tier)
            {
                case MonsterTier.Common:
                    hp = Random.Range(9, 14);
                    break;
                case MonsterTier.Uncommon:
                    hp = Random.Range(13, 18);
                    break;
                case MonsterTier.Rare:
                    hp = Random.Range(17, 23);
                    break;
                case MonsterTier.Legendary:
                    hp = Random.Range(22, 31);
                    break;
                default:
                    hp = 10;
                    break;
            }
        }
    }
}