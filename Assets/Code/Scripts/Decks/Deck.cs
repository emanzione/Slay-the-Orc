using System;
using System.Collections.Generic;
using MHLab.SlayTheOrc.Decks.Cards;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MHLab.SlayTheOrc.Decks
{
    public sealed class Deck : MonoBehaviour
    {
        public List<Card> CardPrefabs;
        public CardPositions CardPositions;
        public Battle Battle;
        public GameObject EmptyCardPlaceholder;
        
        private List<Card> _cards;
        private int _cardTypesAmount;

        private const int AmountOfCardsInHand = 3;
        
        private void Awake()
        {
            _cardTypesAmount = Enum.GetValues(typeof(CardType)).Length;
            
            _cards = new List<Card>(AmountOfCardsInHand);

            for (var i = 0; i < AmountOfCardsInHand; i++)
            {
                var card = GenerateCard();
                card.transform.position = CardPositions.GetPosition(i);
                _cards.Add(card);
            }
            
            EmptyCardPlaceholder.SetActive(false);
        }

        private Card GenerateCard()
        {
            var type = (CardType) Random.Range(0, _cardTypesAmount);
            var prefab = CardPrefabs[(int) type];
            var value = Random.Range(1, 6);

            var card = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            card.Battle = Battle;
            card.SetValue(value);

            return card;
        }

        public void PlayCardAtIndex(int index)
        {
            var card = _cards[index];
            card.BeginPlay();
            
            card.gameObject.SetActive(false);
            EmptyCardPlaceholder.transform.position = card.transform.position;
            EmptyCardPlaceholder.gameObject.SetActive(true);

            _cards[index] = null;
            Destroy(card.gameObject);
        }

        public List<Card> GetCurrentCards()
        {
            return _cards;
        }

        public void DraftCard()
        {
            for (var i = 0; i < AmountOfCardsInHand; i++)
            {
                if (_cards[i] != null) continue;
                
                var card = GenerateCard();
                card.transform.position = CardPositions.GetPosition(i);
                _cards[i] = card;
            }
            
            EmptyCardPlaceholder.SetActive(false);
        }
    }
}