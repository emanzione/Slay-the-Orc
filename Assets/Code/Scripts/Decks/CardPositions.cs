using System.Collections.Generic;
using UnityEngine;

namespace MHLab.SlayTheOrc.Decks
{
    public sealed class CardPositions : MonoBehaviour
    {
        public List<Transform> Positions;

        public Vector2 GetPosition(int index)
        {
            return Positions[index].position;
        }
    }
}