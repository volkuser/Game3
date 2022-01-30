using UnityEngine;

namespace Objects.Player
{
    public class AnimationManager : MonoBehaviour
    {
        private Animator _animation;
        private static readonly int NameAnimation 
            = Animator.StringToHash("Animation");

        private void Start()
        {
            _animation = GetComponent<Animator>();
        }

        public void SetAnimationIdle() => _animation.
            SetInteger(NameAnimation, 0);
        public void SetAnimationRun() => _animation.
            SetInteger(NameAnimation, 1);
        public void SetAnimationJump() => _animation.
            SetInteger(NameAnimation, 2);
        public void SetAnimationAttack() => _animation.
            SetInteger(NameAnimation, 3);
    }
}
