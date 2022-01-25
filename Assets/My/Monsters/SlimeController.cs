using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace My.Monsters
{
    public class SlimeController : MonoBehaviour, IAttackable
    {
        public float viewingDistance = 10f;
        public float attackDistance = 2f;
        public float attackRange = 0.7f;
        public int attackCountDownSeconds = 1;
        
        public GameObject attackPoint;
        public int health = 30;
        //public ParticleSystem damageParticle;
        
        private bool _enableAttack = true;
        private float _distanceToPlayer;
        
        private Transform _target;
        private NavMeshAgent _agent;
        private Animator _animation;
        public LayerMask playerLayer;
        
        private GameManager _gameManager;
        
        private static readonly int AnimationName 
            = Animator.StringToHash("Animation");

        void Start()
        {
            _gameManager = GameManager.GETManagerInstance();
            _target = _gameManager.player.transform;
            _agent = GetComponent<NavMeshAgent>();
            _animation = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            // change slime animation
            SetAnimation();
            
            _distanceToPlayer = Vector3.Distance(_target.position, 
                transform.position);
            if (_distanceToPlayer <= viewingDistance)
            {
                var position = _target.position;
                _agent.SetDestination(position);
                transform.LookAt(position);

                if (_distanceToPlayer <= attackDistance && _enableAttack) 
                    StartCoroutine(AttackCountdown());
            }
            
            if (health <= 0) Death();
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            var position = attackPoint.transform.position;
            Gizmos.DrawSphere(position, attackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(position, viewingDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, attackDistance);
        }
        
        private void SetAnimation()
        {
            if (_distanceToPlayer <= attackDistance && _enableAttack)  
                _animation.SetInteger(AnimationName, 2);
            else
            {
                _animation.SetInteger(AnimationName, 
                    _distanceToPlayer <= viewingDistance ? 1 : 0);
            }
        }
        
        public void DealDamage(int count)
        {
            health -= count;
            //damageParticle.Play();
        }
        
        private void Attack()
        {
            Collider[] hitColliders = Physics.
                OverlapSphere(attackPoint.transform.position, 
                    attackRange, playerLayer);
            _enableAttack = true;
            foreach (Collider unused in hitColliders)
            {
                _gameManager.DamagePlayer(10);
            }
        }

        private IEnumerator AttackCountdown()
        {
            _enableAttack = false;
            int counter = attackCountDownSeconds;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            Attack(); 
        }
        
        private void Death()
        {
            //damageParticle.transform.parent = null;
            //damageParticle.Play();
            Destroy(gameObject);
            _gameManager.AddScore(100);
        }
    }
}
