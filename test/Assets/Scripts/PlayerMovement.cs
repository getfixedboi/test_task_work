using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MoveInput;
    [HideInInspector]
    public CharacterController Character;

    [Header("Variables")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _jumpHeight;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckDistance = 0.1f;
    public static Vector3 Velocity;
    public static bool IsGrounded = false;

    [Inject]
    private FloatingJoystick _joystick;

    private void Awake()
    {
        Character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_joystick == null) { return; }
        
        float axis = _joystick.Horizontal;
        float axis2 = _joystick.Vertical;
        MoveInput = new Vector3(axis, 0f, axis2);
        MoveInput.Normalize();

        Vector3 direction = transform.right * axis + transform.forward * axis2;
        direction = Vector3.ClampMagnitude(direction, 1f);

        if (axis != 0 || axis2 != 0)
        {
            Character.Move(direction * _speed * Time.deltaTime);
        }
        else
        {
            MoveInput = Vector3.zero;
            Character.Move(Vector3.zero * Time.deltaTime);
        }

        IsGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (!IsGrounded)
        {
            Velocity.y += _gravity * Time.deltaTime;
        }

        Character.Move(Velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (IsGrounded)
        {
            Velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}