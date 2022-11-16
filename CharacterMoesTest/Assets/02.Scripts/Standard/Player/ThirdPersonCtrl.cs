using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCtrl : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("ĳ���� �̵� �ӵ�(�⺻ �̵� - �޸���)")]
    public float MoveSpeed = 6f;

    [Tooltip("�ȱ� �̵� �ӵ�")]
    public float WalkSpeed = 2f;

    [Tooltip("ȸ���� �ε巴�� �ϴ� �ð� ��")]
    [Range(0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("�ӵ� ���� ���")]
    public float SpeedChangeRate = 10f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("���� ���� ����")]
    public float JumpHeight = 1.2f;

    [Tooltip("�߷� ��(RigidBody�� ������� �����Ƿ�)")]
    public float Gravity = -15f;

    [Tooltip("������ �ٽ��ϴµ� �ʿ��� �ð�, 0�̸� �ٷ� ���� ��")]
    public float JumpTimeout = 0.5f;

    [Tooltip("���ϻ��·� ��ȯ�Ǵµ� �ʿ��� �ð�, ��ܿ��� �������� ���")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("ĳ���Ͱ� ���� ������ ���� ����")]
    public bool isGrounded = true;

    [Tooltip("��ģ ���� ����� ��")]
    public float GroundedOffset = -0.14f;

    [Tooltip("������ üũ�ϱ� ���� radius")]
    public float GroundedRadius = 0.28f;

    [Tooltip("�� ���̾�")]
    public LayerMask GroundLayers;

    //[Header("Cinemachine")]
    //[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    //public GameObject CinemachineCameraTarget;

    //[Tooltip("How far in degrees can you move the camera up")]
    //public float TopClamp = 70.0f;

    //[Tooltip("How far in degrees can you move the camera down")]
    //public float BottomClamp = -30.0f;

    //[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    //public float CameraAngleOverride = 0.0f;

    //[Tooltip("For locking the camera position on all axis")]
    //public bool LockCameraPosition = false;

    //// cinemachine
    //private float _cinemachineTargetYaw;
    //private float _cinemachineTargetPitch;

    // �÷��̾�
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
        //_cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        hasAnimator = TryGetComponent(out animator);
        controller = GetComponent<CharacterController>();
        input = GetComponent<StandardInput>(); 
        playerInput = GetComponent<PlayerInput>();

        AssignAnimationIDs();

        // Ÿ�Ӿƿ� �� �ʱ�ȭ
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
        //CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        hashSpeed = Animator.StringToHash("Speed");
        hashGrounded = Animator.StringToHash("Grounded");
        hashJump = Animator.StringToHash("Jump");
        hashFreeFall = Animator.StringToHash("FreeFall");
        hashMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    void GroundedCheck()    // ���� ��Ѵ��� üũ �ϴ� �Լ�
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z); // ������ �Ÿ��� üũ�ϴ� ����
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore); // ���� ������� true�� ���� �ƴϸ� false

        if (hasAnimator)
        {
            // ���� ������ �ִϸ��̼� ����
            animator.SetBool(hashGrounded, isGrounded);
        }
    }

    //private void CameraRotation()
    //{
    //    // if there is an input and camera position is not fixed
    //    if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
    //    {
    //        //Don't multiply mouse input by Time.deltaTime;
    //        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

    //        _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
    //        _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
    //    }

    //    // clamp our rotations so our values are limited 360 degrees
    //    _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
    //    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

    //    // Cinemachine will follow this target
    //    CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
    //        _cinemachineTargetYaw, 0.0f);
    //}

    void Move()
    {
        
        // �ȱ� ������ ���� �̵��ӵ� �Ҵ�
        float targetSpeed = input.isWalk ? WalkSpeed : MoveSpeed;

        // �̵����� ������ �ӵ��� 0
        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        // �������� ���� �̵� �ӵ�
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        // ���� �Ǵ� ������ ��� �ӵ�
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

        // �Է� ���� ����ȭ
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // �����̰� �ִٸ� ȸ��
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
            // FallTimeout �ð� ����
            _fallTimeoutDelta = FallTimeout;

            // ���� ������Ƿ� ���� �� ���ϻ��� ����
            if (hasAnimator)
            {
                animator.SetBool(hashJump, false);
                animator.SetBool(hashFreeFall, false);
            }

            // �ϰ��ӵ��� 0���� ������ -2�� ����
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
        else // ���ϻ���
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
