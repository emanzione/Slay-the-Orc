using UnityEngine;
using UnityEngine.SceneManagement;

namespace MHLab.SlayTheOrc.Utilities
{
    public sealed class SceneLoader : MonoBehaviour
    {
        public string Scene;

        public void Load()
        {
            SceneManager.LoadScene(Scene);
        }
    }
}