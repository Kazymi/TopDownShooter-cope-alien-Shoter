using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; }

    public bool IsRunning { get; private set; }

    public bool InInteractItem { get; private set; }

    private void Update()
    {
        IsRunning = Input.GetKey(KeyCode.LeftShift);
        InInteractItem = Input.GetKeyDown(KeyCode.Space);
        MoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}