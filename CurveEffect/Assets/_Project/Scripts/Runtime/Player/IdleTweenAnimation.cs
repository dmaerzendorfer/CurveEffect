using EditorAttributes;
using PrimeTween;
using UnityEngine;

namespace _Project.Scripts.Runtime.Player
{
    public class IdleTweenAnimation : TweenAnimation
    {
        //todo: perfect animation to follow animation principles for squash and stretch
       
        public TweenSettings<Vector3> scaleTweenSettings;
        private Tween _animationTween;


        public override void SetNormalizedAnimationSpeed(float speed)
        {
            if (_animationTween.isAlive)
            {
                _animationTween.timeScale = _normalizedAnimationSpeed;
            }
        }

        [Button]
        public override void PlayAnimation()
        {
            if (_animationTween.isAlive) return;
            _animationTween = Tween.Scale(targetObject.transform, scaleTweenSettings);
            NormalizedAnimationSpeed = _normalizedAnimationSpeed;
        }
        [Button]
        public override void StopAnimation()
        {
            if (_animationTween.isAlive)
            {
                _animationTween.Complete();
                targetObject.transform.localScale = scaleTweenSettings.startValue;
            }
        }

      
    }
}