using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class Management : MonoBehaviour
    {
        public void OpenLevel(int levelNumber)
        {
            SceneManager.LoadScene(levelNumber);
        }
    }
}
