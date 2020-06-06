using MHLab.SlayTheOrc.Decks.Cards;
using UnityEngine;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class MonsterNextMove : MonoBehaviour
    {
        public SpriteRenderer CurrentSprite;
        public TextMesh CurrentValueText;

        public Sprite AttackSprite;
        public Sprite ShieldSprite;
        public Sprite ReflectSprite;
        
        public void SetMove(in BattleAction action)
        {
            CurrentValueText.text = action.Value.ToString();

            switch (action.Type)
            {
                case ActionType.Attack:
                    CurrentSprite.sprite = AttackSprite;
                    break;
                case ActionType.Shield:
                    CurrentSprite.sprite = ShieldSprite;
                    break;
                case ActionType.Reflect:
                    CurrentSprite.sprite = ReflectSprite;
                    break;
            }
        }
    }
}