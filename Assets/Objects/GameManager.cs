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
            if (player.transform.position.y < -5) 
                Death();
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
            SceneManager.LoadScene(SceneManager.
                GetActiveScene().buildIndex);
        }
    }
}
