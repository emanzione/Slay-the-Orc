using UnityEngine;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class Resourcebar : MonoBehaviour
    {
        public Transform InnerBar;
        public TextMesh Text;

        private Vector3 InitialScale;
        
        private void Awake()
        {
            InitialScale = InnerBar.localScale;
        }

        public void SetValue(int current, int max)
        {
            var currentRatio = (float) current / (float) max;
            var actualRatio = currentRatio * InitialScale.x;

            InnerBar.localScale = new Vector3(actualRatio, InitialScale.y, InitialScale.z);

            Text.text = $"{current} / {max}";
        }
    }
}