using UnityEngine;
using UnityEngine.VFX;

namespace _Project.Scripts.Runtime.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        //todo: this needs refactoring, the movement coord swizzling seems a bit confusing 
        //also the backside rendering of the sprite looks wrong
        [Header("Animations")]
        public IdleTweenAnimation idleTweenAnimation;

        public VisualEffect dustTrail;
        public float maxDustRate = 12f;

        public RunTweenAnimation runTweenAnimation;


        [Header("Movement")]
        [Tooltip("Maximum movement speed (units/sec)")]
        public float maxSpeed = 5f;

        [Tooltip(
            "Curve used to ease between speeds when accelerating. X = normalized time (0..1), Y = interpolation (0..1)")]
        public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Tooltip("Time in seconds it takes to transition between speeds when accelerating. Set to 0 for instant.)")]
        public float rampDuration = 0.5f;

        [Tooltip("Curve used to ease between speeds when decelerating.")]
        public AnimationCurve decelSpeedCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Tooltip("Time in seconds it takes to transition between speeds when decelerating. Set to 0 for instant.)")]
        public float decelRampDuration = 0.5f;

        [Tooltip("When true, input is interpreted in local space (relative to transform). When false, world space.")]
        public bool useLocalSpace = true;

        // debug/read-only
        [Header("Debug")]
        [Tooltip("Current interpolated speed (read-only at runtime)")]
        public float currentSpeedDebug;

        [HideInInspector]
        public Vector2 movementInput;

        private Rigidbody _rb;

        // ramp state
        private Vector3 _desiredDir = Vector3.zero;
        private Vector3 _lastNonZeroDir = Vector3.forward;
        private float _desiredSpeed = 0f;
        private float _startSpeed = 0f;
        private float _currentSpeed = 0f;
        private float _rampTimer = 0f;
        private float _lastDesiredSpeed = -1f;
        private bool _lastWasAccelerating = true;


        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void FixedUpdate()
        {
            // Map 2D input (x, y) -> 3D movement (y, 0, -x) to preserve original axis mapping
            var inputDir = new Vector3(movementInput.y, 0f, -movementInput.x);
            float inputMagnitude = movementInput.magnitude; // 0..1 (if using normalized input from Input System)
            
            if (movementInput.x != 0f)
                transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, -movementInput.x));


            // Determine desired speed and direction
            _desiredSpeed = Mathf.Clamp01(inputMagnitude) * maxSpeed;

            if (inputMagnitude > 0.0001f)
            {
                // convert to local space if requested
                var worldDir = useLocalSpace ? transform.TransformDirection(inputDir) : inputDir;
                _desiredDir = worldDir.normalized;
                _lastNonZeroDir = _desiredDir;
            }
            else
            {
                // when there's no input, keep last non-zero direction so we decelerate along the same vector
                _desiredDir = _lastNonZeroDir;
            }

            // If desired speed changed since last frame, start a new ramp and decide accel vs decel
            if (!Mathf.Approximately(_desiredSpeed, _lastDesiredSpeed))
            {
                _startSpeed = _currentSpeed;
                _rampTimer = 0f;
                _lastWasAccelerating = _desiredSpeed > _startSpeed;
                _lastDesiredSpeed = _desiredSpeed;
            }

            // choose curve and duration depending on whether we're accelerating or decelerating
            var selectedCurve = _lastWasAccelerating ? speedCurve : decelSpeedCurve;
            var selectedDuration = _lastWasAccelerating ? rampDuration : decelRampDuration;

            // advance ramp timer and sample curve
            _rampTimer += Time.fixedDeltaTime;
            float t = (selectedDuration > 0f) ? Mathf.Clamp01(_rampTimer / selectedDuration) : 1f;
            float curveT = (selectedCurve != null) ? selectedCurve.Evaluate(t) : t;
            _currentSpeed = Mathf.Lerp(_startSpeed, _desiredSpeed, curveT);

            // apply movement (preserve Rigidbody Y velocity if you want physics like gravity)
            var velocity = _desiredDir * _currentSpeed; // units/sec
            // _rb.MovePosition(_rb.position + velocity * Time.fixedDeltaTime);
            _rb.linearVelocity = velocity * Time.fixedDeltaTime;

            // expose debug value
            currentSpeedDebug = _currentSpeed;

            DoAnimatonLogic(t);
        }

        private void DoAnimatonLogic(float t)
        {
            if (_desiredSpeed <= 0.0001f)
            {
                //stop run
                runTweenAnimation.StopAnimation();
                dustTrail.SetFloat("SpawnRate", 0f);

                //start idle
                idleTweenAnimation.NormalizedAnimationSpeed = 1f;
                idleTweenAnimation.PlayAnimation();
            }
            else
            {
                //stop idle
                idleTweenAnimation.StopAnimation();

                //start run
                runTweenAnimation.PlayAnimation();
                runTweenAnimation.SetNormalizedAnimationSpeed(t);
                dustTrail.SetFloat("SpawnRate",
                    Mathf.Lerp(0, maxDustRate, t)); //todo: make it also depend on speed if you want more dynamic effect
            }
        }
    }
}