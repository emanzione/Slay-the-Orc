using UnityEngine;

namespace MHLab.SlayTheOrc.Decks
{
    public sealed class CardSelection : MonoBehaviour
    {
        public CardPositions Positions;
        public Deck Deck;
        public Battle Battle;
        
        public int CurrentIndex;

        private Transform _selectionTransform;
        
        private void Awake()
        {
            _selectionTransform = transform;

            MoveToPosition();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                CurrentIndex++;

                if (CurrentIndex >= 3)
                {
                    CurrentIndex = 0;
                }
                
                MoveToPosition();
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                CurrentIndex--;
                
                if (CurrentIndex < 0)
                {
                    CurrentIndex = 2;
                }
                
                MoveToPosition();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
            {
                if (Battle.IsCompletingTurn) return;

                if (Battle.IsPlayerTurn())
                {
                    Battle.IsCompletingTurn = true;
                    Deck.PlayCardAtIndex(CurrentIndex);
                }
            }
        }

        private void MoveToPosition()
        {
            var position = Positions.GetPosition(CurrentIndex);
            _selectionTransform.position = position;
        }
    }
}