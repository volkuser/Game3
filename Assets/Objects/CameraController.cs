using UnityEngine;

namespace Objects
{
    public class CameraController : MonoBehaviour
    {
        public GameObject player;
    
        float smoothing = 10f;
        
        Vector3 _offset;
        
        void Start()
        {
            _offset = gameObject.transform.position - 
                     player.transform.position;
        }

        void Update()
        {
            gameObject.transform.position = 
                Vector3.Lerp(gameObject.transform.position, 
                    player.transform.position + _offset, smoothing);
        }
    }
}
