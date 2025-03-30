using UnityEngine;

public class TouchMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float minMovementThreshold = 4f;
    [SerializeField] private float joystickDeadzone = 0.2f;
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;

    private Vector2 lastTouchPosition;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;
    private bool isMoving = false;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            StopMovement();
            return;
        }

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                lastTouchPosition = touch.position;
                isMoving = true;
                break;

            case TouchPhase.Moved:
                ProcessTouchMovement(touch.position);
                break;

            case TouchPhase.Ended:
                StopMovement();
                break;
        }
    }

    private void ProcessTouchMovement(Vector2 touchPosition)
    {
        Vector2 delta = touchPosition - lastTouchPosition;
        lastTouchPosition = touchPosition;

        if (delta.sqrMagnitude < minMovementThreshold) return;

        Vector2 joystickInput = new Vector2(delta.x, delta.y).normalized;

        if (joystickInput.magnitude < joystickDeadzone)
        {
            joystickInput = Vector2.zero;
        }

        moveDirection = new Vector3(joystickInput.x, 0f, joystickInput.y).normalized;

        float speed = moveDirection.magnitude * moveSpeed;

        velocity = Vector3.Lerp(velocity, moveDirection * speed, Time.deltaTime * accelerationSpeed);

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        bool Run = velocity.magnitude > 0f;

        animator.SetBool("Run", Run);
    }

    private void StopMovement()
    {
        if (isMoving)
        {
            isMoving = false;
            animator.SetBool("Run", false);
            moveDirection = Vector3.zero;
            velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            MoveCharacter();
            RotateCharacter();
        }
    }

    private void MoveCharacter()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private void RotateCharacter()
    {
        if (moveDirection.sqrMagnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        }
    }
}
