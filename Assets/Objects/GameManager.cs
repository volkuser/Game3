using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Objects
{
    public class GameManager : MonoBehaviour
    {
        public int gameLevel = 1;
        
        // player state
        private int _totalScore;
        private int _health = 100;
        
        // player state on display
        public Text scoreText;
        public Slider healthBar;
        public Image deathWindow;
        public Text targetContent;
        
        public GameObject player;

        private static GameManager _managerInstance;

        public static GameManager GETManagerInstance()
        {
            return _managerInstance;
        }

        void FixedUpdate()
        {
            if (gameLevel == 1)
            {
                if (player.transform.position.y < -8)
                    Death();
                EndLevel(gameLevel);
            } else if (gameLevel == 2 || gameLevel == 3)
            {
                if (player.transform.position.y < 48)
                    Death();
                EndLevel(gameLevel);
            }
        }

        public void Awake()
        {
            _managerInstance = this;
        }
        
        public void AddScore(int count)
        {
            _totalScore += count;
            scoreText.text = "Score: " + _totalScore;
        }
        
        public void DamagePlayer(int count)
        {
            if (_health > 0)
            {
                _health -= count;
                healthBar.value -= count;
            }
            else Death();
        }
        
        private void Death()
        {
            player.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);
            targetContent.gameObject.SetActive(false);
            
            deathWindow.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(gameLevel);
        }

        private void EndLevel(int level)
        {
            switch (level)
            {
                case 1: if (_totalScore == 400
                    && player.transform.position.y < -7)
                        SceneManager.LoadScene(2);
                    break;
                case 2: if (_totalScore > 99
                    && player.transform.position.y < 51
                    && player.transform.position.z > 42) 
                        SceneManager.LoadScene(3);
                    break;
                case 3:
                    if (_totalScore > 199
                    ) SceneManager.LoadScene(4);
                    break;
            }
        }
    }
}
