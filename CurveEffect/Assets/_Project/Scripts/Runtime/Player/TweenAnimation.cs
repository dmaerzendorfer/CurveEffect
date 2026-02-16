using UnityEngine;

namespace _Project.Scripts.Runtime.Player
{
    public abstract class TweenAnimation : MonoBehaviour
    {
        public GameObject targetObject;
        public bool shouldPlayOnStart = true;

        public float NormalizedAnimationSpeed
        {
            get => _normalizedAnimationSpeed;
            set
            {
                _normalizedAnimationSpeed = Mathf.Clamp01(value);
                SetNormalizedAnimationSpeed(_normalizedAnimationSpeed);
            }
        }

        protected float _normalizedAnimationSpeed = 1f;

        public virtual void Start()
        {
            if (shouldPlayOnStart)
            {
                PlayAnimation();
            }
        }

        public virtual void SetNormalizedAnimationSpeed(float speed) { }

        public virtual void PlayAnimation() { }

        public virtual void StopAnimation() { }

        private void OnDestroy()
        {
            StopAnimation();
        }
    }
}