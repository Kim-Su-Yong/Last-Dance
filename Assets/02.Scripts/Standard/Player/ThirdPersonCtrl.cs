using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCtrl : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("캐릭터 이동 속도(기본 이동 - 달리기)")]
    public float MoveSpeed = 6f;

    [Tooltip("걷기 이동 속도")]
    public float WalkSpeed = 2f;

    [Tooltip("회전시 부드럽게 하는 시간 값")]
    [Range(0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("속도 변경 계수")]
    public float SpeedChangeRate = 10f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("점프 높이 설정")]
    public float JumpHeight = 1.2f;

    [Tooltip("중력 값(RigidBody를 사용하지 않으므로)")]
    public float Gravity = -15f;

    [Tooltip("점프를 다시하는데 필요한 시간, 0이면 바로 점프 뜀")]
    public float JumpTimeout = 0.5f;

    [Tooltip("낙하상태로 전환되는데 필요한 시간, 계단에서 떨어질때 사용")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("캐릭터가 땅에 착지한 상태 유무")]
    public bool isGrounded = true;

    [Tooltip("거친 땅에 사용할 값")]
    public float GroundedOffset = -0.14f;

    [Tooltip("땅인지 체크하기 위한 radius")]
    public float GroundedRadius = 0.28f;

    [Tooltip("땅 레이어")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("타겟을 설정하면 가상 카메라가 따라감")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [Header("제어 변수")]
    public bool IsAction;

    // 플레이어
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animation IDs
    int hashSpeed;
    int hashGrounded;
    int hashJump;
    int hashFreeFall;
    int hashMotionSpeed;

    PlayerInput playerInput;
    Animator animator;
    CharacterController controller;
    StandardInput input;
    private GameObject mainCamera;

    const float _threshold = 0.01f;
    bool hasAnimator;

    private bool IsCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        hasAnimator = TryGetComponent(out animator);
        controller = GetComponent<CharacterController>();
        input = GetComponent<StandardInput>(); 
        playerInput = GetComponent<PlayerInput>();

        AssignAnimationIDs();

        // 타임아웃 값 초기화
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    void Update()
    {
        hasAnimator = TryGetComponent(out animator);

        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        hashSpeed = Animator.StringToHash("Speed");
        hashGrounded = Animator.StringToHash("Grounded");
        hashJump = Animator.StringToHash("Jump");
        hashFreeFall = Animator.StringToHash("FreeFall");
        hashMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    void GroundedCheck()    // 땅에 닿앗는지 체크 하는 함수
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z); // 땅과의 거리를 체크하는 벡터
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore); // 땅에 닿았으면 true로 변경 아니면 false

        if (hasAnimator)
        {
            // 땅에 착지한 애니메이션 실행
            animator.SetBool(hashGrounded, isGrounded);
        }
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if(input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    void Move()
    {
        if (GetComponent<PlayerDamage>().isDie)
            return;
        if (IsAction == true)
            return;

        // 걷기 유무에 따른 이동속도 할당
        float targetSpeed = input.isWalk ? WalkSpeed : MoveSpeed;

        // 이동하지 않으면 속도는 0
        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        // 땅에서의 수평 이동 속도
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        // 가속 또는 감속의 경우 속도
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // 입력 방향 정규화
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // 움직이고 있다면 회전
        if (input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (hasAnimator)
        {
            animator.SetFloat(hashSpeed, _animationBlend);
            animator.SetFloat(hashMotionSpeed, inputMagnitude);
        }
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            // FallTimeout 시간 리셋
            _fallTimeoutDelta = FallTimeout;

            // 땅에 닿았으므로 점프 및 낙하상태 해제
            if (hasAnimator)
            {
                animator.SetBool(hashJump, false);
                animator.SetBool(hashFreeFall, false);
            }

            // 하강속도가 0보다 작으면 -2로 변경
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (input.isJump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(hashJump, true);
                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else // 낙하상태
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(hashFreeFall, true);
                }
            }

            // if we are not grounded, do not jump
            input.isJump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }
    }


    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
        }
    }
}
