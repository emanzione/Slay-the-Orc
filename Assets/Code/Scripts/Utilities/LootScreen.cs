using MHLab.SlayTheOrc.Maps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class LootScreen : MonoBehaviour
    {
        public Text MainText;
        public Text GoldValue;
        public Text PotionsValue;
        
        private static readonly string[] Quotes = new string[]
        {
            "You did it! For now...",
            "Oh, damn: it was so close...",
            "It was so close this time...",
            "I thought you were going to die...",
            "If you die, can I take your loot?",
            "Keep going, don't stop!",
            "It does not seem like a nice loot...",
            "It seems really a poor loot...",
            "You've been lucky.",
            "You've been really lucky.",
            "You will not be so lucky next time...",
            "You got it! But pay attention to that trap on your left...",
            "Let's check what you've found.",
        };
        
        private void Awake()
        {
            var loot = LootManager.GetLootForMonster(MonsterManager.LastGeneratedMonster);
            GoldValue.text = loot.Gold.ToString();
            PotionsValue.text = loot.SmallPotions.ToString();

            MainText.text = GetRandomQuote();
            
            LootManager.AddLootToBag(loot);
        }

        private string GetRandomQuote()
        {
            return Quotes[Random.Range(0, Quotes.Length)];
        }

        public void GoToLevelSelectionScreen()
        {
            var node = MapNodesManager.GetMapNode(PlayerManager.NodeIndexInMap);
            node.MarkAsSolved();

            MapNodesManager.Instance.gameObject.SetActive(true);
            
            SceneManager.LoadScene("LevelSelection");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
            {
                GoToLevelSelectionScreen();
            }
        }
    }
}