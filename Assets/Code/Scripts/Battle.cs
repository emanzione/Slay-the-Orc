using MHLab.SlayTheOrc.Decks;
using MHLab.SlayTheOrc.Utilities;
using UnityEngine;

namespace MHLab.SlayTheOrc
{
    public enum BattleTurn
    {
        Player,
        Monster,
        Finished
    }
    
    public sealed class Battle : MonoBehaviour
    {
        public BattleTurn CurrentTurn;
        public bool IsCompletingTurn;

        public PlayerInBattle Player;
        public Monster Monster;
        public Deck Deck;

        public FloatingSprite PlayerTurnCursor;
        public FloatingSprite MonsterTurnCursor;

        private void Awake()
        {
            PlayerTurnCursor.gameObject.SetActive(true);
            MonsterTurnCursor.gameObject.SetActive(false);
        }

        private void Start()
        {
            CurrentTurn = BattleTurn.Player;
            Monster.SetNextMove();
        }

        public bool IsPlayerTurn()
        {
            return CurrentTurn == BattleTurn.Player;
        }

        public void ChangeTurn()
        {
            if (CurrentTurn == BattleTurn.Player)
            {
                PlayerTurnCursor.gameObject.SetActive(false);
                MonsterTurnCursor.gameObject.SetActive(true);
                CurrentTurn = BattleTurn.Monster;
                IsCompletingTurn = true;
                Monster.ClearBuffs();
                Monster.PerformBattleAction();
            }
            else
            {
                PlayerTurnCursor.gameObject.SetActive(true);
                MonsterTurnCursor.gameObject.SetActive(false);
                CurrentTurn = BattleTurn.Player;
                Monster.SetNextMove();
                Player.ClearBuffs();
                Deck.DraftCard();
            }
        }

        public void FinishBattle()
        {
            PlayerTurnCursor.gameObject.SetActive(false);
            MonsterTurnCursor.gameObject.SetActive(false);
            CurrentTurn = BattleTurn.Finished;
        }
    }
}