using UnityEngine;

namespace Objects
{
    public class FoodController : MonoBehaviour
    {
        private GameManager _gameManager;
        
        void Start()
        {
            _gameManager = GameObject.Find("GameManager").
                GetComponent<GameManager>();
        }

        void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
            _gameManager.AddScore(100);
        }
    }
}
