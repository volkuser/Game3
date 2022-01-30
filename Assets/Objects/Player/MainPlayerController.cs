using System.Collections;
using UnityEngine;

namespace Objects.Player
{
    public class MainPlayerController : MonoBehaviour
    {
        // player "body"
        public new Rigidbody rigidbody;
        private AnimationManager _animationManager;

        // player parameters
        private float movementSpeed = 3.0f;
        private float jumpForce = 1.5f;
        
        public float destinationToGround = 0.1f;
        
        // player state
        public bool isGrounded = true;
        
        // for battle
        public int strength = 10;
        public GameObject attackPoint;
        public float attackRange = 0.7f;
        public LayerMask attackedLayer;
        public int attackCountdownSeconds = 1;
        private bool _enableAttack = true;
        
        void Start()
        {
            // initializing of class objects
            rigidbody = GetComponent<Rigidbody>();
            _animationManager = GetComponent<AnimationManager>();
            
            
        }

        void FixedUpdate()
        {
            // check of player on ground
            GroundCheck();
            
            if (Input.GetKey(KeyCode.Space) && isGrounded) Jump();
            
            if (Input.GetKey(KeyCode.Mouse0) && _enableAttack) Attack();
            
            // change player position
            rigidbody.MovePosition(CalculateMovement());
            // change player rotation
            SetRotation();

            // change player animation
            SetPlayerAnimation();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var position = transform.position;
            Gizmos.DrawLine(position, position 
                                      + (Vector3.down * destinationToGround));
            Gizmos.DrawSphere(attackPoint.transform.position, attackRange);
        }
        
        private void GroundCheck()
        {
            isGrounded = Physics.Raycast(transform.position, 
                Vector3.down, destinationToGround);
        }
        
        private void Jump()
        {
            rigidbody.AddForce(Vector3.up * jumpForce, 
                ForceMode.Impulse);
        }
        
        private void Attack()
        {
            Collider[] hitColliders = Physics.OverlapSphere(attackPoint.
                transform.position, attackRange, attackedLayer);
            foreach (Collider hitCollider in hitColliders)
            {
                IAttackable attacker = hitCollider.gameObject.
                    GetComponent<IAttackable>();
                attacker.DealDamage(strength);
            }

            _enableAttack = false;

            StartCoroutine(AttackCountdown());
        }

        private IEnumerator AttackCountdown()
        {
            int counter = attackCountdownSeconds;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            _enableAttack = true;
        }
        
        Vector3 CalculateMovement()
        {
            // current player directions
            float horizontalDirection = Input.GetAxis("Horizontal");
            float verticalDirection = Input.GetAxis("Vertical");

            // find of current player position
            return rigidbody.transform.position + 
                   new Vector3(horizontalDirection, 0, 
                       verticalDirection) * (Time.fixedDeltaTime * movementSpeed);
        }
        
        private void SetRotation()
        {
            Plane playerPlane = new Plane(Vector3.up, 
                transform.position);
            if (Camera.main is { })
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (playerPlane.Raycast(ray, out var hitDistance))
                {
                    Vector3 targetPoint = ray.GetPoint(hitDistance);
                    Quaternion targetRotation = Quaternion.
                        LookRotation(targetPoint - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, 
                        targetRotation, movementSpeed * Time.deltaTime);
                }
            }
        }
        
        private bool IsMoving()
        {
            return new Vector3(Input.GetAxis("Horizontal"), 
                0, Input.GetAxis("Vertical")) != Vector3.zero;
        }
        
        private void SetPlayerAnimation()
        {
            if (isGrounded)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                    _animationManager.SetAnimationAttack();
                else 
                {
                    if (IsMoving()) _animationManager.SetAnimationRun();
                    else _animationManager.SetAnimationIdle();
                }
            }
            else _animationManager.SetAnimationJump();
        }
    }
}
