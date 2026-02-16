using EditorAttributes;
using PrimeTween;
using UnityEngine;

namespace _Project.Scripts.Runtime.Player
{
    public class RunTweenAnimation : TweenAnimation
    {
        public TweenSettings<Vector3> localPositionTweenSettings;
        public TweenSettings<Vector3> localRotationTweenSettings;

        private Sequence _tweenSequence;

        public override void Start()
        {
            base.Start();
            _tweenSequence = Sequence.Create(1, CycleMode.Yoyo)
                .Group(Tween.LocalRotation(targetObject.transform, localRotationTweenSettings))
                .Group(Tween.LocalPosition(targetObject.transform, localPositionTweenSettings));
            _tweenSequence.isPaused = true;
            _tweenSequence.SetRemainingCycles(-1);
        }

        [Button]
        public override void PlayAnimation()
        {
            _tweenSequence.isPaused = false;
        }

        [Button]
        public override void StopAnimation()
        {
            if (!_tweenSequence.isAlive) return;
            _tweenSequence.isPaused = true;
            targetObject.transform.localPosition = localPositionTweenSettings.startValue;
            targetObject.transform.localRotation = Quaternion.Euler(localRotationTweenSettings.startValue);
        }

        public override void SetNormalizedAnimationSpeed(float speed)
        {
            if (_tweenSequence.isAlive)
            {
                _tweenSequence.timeScale = _normalizedAnimationSpeed;
            }
        }
    }
}