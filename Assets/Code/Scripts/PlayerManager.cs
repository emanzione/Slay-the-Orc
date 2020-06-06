using UnityEngine;

namespace MHLab.SlayTheOrc
{
    public static class PlayerManager
    {
        public static int NodeIndexInMap;
        
        public static int MaxHP;


        static PlayerManager()
        {
            MaxHP = Random.Range(12, 16);
        }
    }
}