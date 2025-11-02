using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputReader InputReader { get; private set; }
    public Vector2 MoveInput, LookInput;

    void Awake()
    {
        InputReader = GetComponent<InputReader>();
    }

    private void Update()
    {
        GetInput();
    }


    private void GetInput()
    {
        MoveInput = InputReader.MovementInput;
        LookInput = InputReader.LookInput;
    }
}
