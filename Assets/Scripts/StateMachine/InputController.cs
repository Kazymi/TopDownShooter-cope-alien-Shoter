using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private bool IsMobileController;

    [HideIf("IsMobileController", false)] [SerializeField]
    private Joystick joystick;

    [HideIf("IsMobileController", false)] [SerializeField]
    private GameButton interatctable;

    [HideIf("IsMobileController", false)] [SerializeField]
    private GameButton drop;

    [HideIf("IsMobileController", false)] [SerializeField]
    private GameButton isRunning;

    [HideIf("IsMobileController", false)] [SerializeField]
    private GameButton isAttacking;


    public Vector3 MoveDirection { get; private set; }

    public bool IsRunning { get; private set; }

    public bool InInteractItem { get; private set; }
    public bool IsDropHandItem { get; private set; }
    public bool IsAttack { get; private set; }

    private void Update()
    {
        if (IsMobileController)
        {
            IsAttack = isAttacking.IsClicked;
            if (isRunning.IsPointerDown)
            {
                IsRunning = !IsRunning;
            }

            IsDropHandItem = drop.IsPointerDown;
            InInteractItem = interatctable.IsPointerDown;
            MoveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            return;
        }

        IsAttack = Input.GetMouseButton(0);
        IsRunning = Input.GetKey(KeyCode.LeftShift);
        InInteractItem = Input.GetKeyDown(KeyCode.Space);
        IsDropHandItem = Input.GetKeyDown(KeyCode.Q);
        MoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}