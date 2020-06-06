using UnityEngine;

namespace MHLab.SlayTheOrc.Decks.Cards
{
    public abstract class Card : MonoBehaviour
    {
        public int Value;
        public TextMesh Text;
        public Battle Battle;

        public void BeginPlay()
        {
            Play();
        }
        
        public abstract void Play();

        public void SetValue(int value)
        {
            Value = value;
            Text.text = value.ToString();
        }
    }
}