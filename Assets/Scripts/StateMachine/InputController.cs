using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; }

    public bool IsRolling { get; private set; }

    private void Update()
    {
        IsRolling = Input.GetKeyDown(KeyCode.Space);
        MoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}