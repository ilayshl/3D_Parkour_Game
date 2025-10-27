/* using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public float MoveSpeed { get => moveSpeed; }
    [SerializeField] private float moveSpeed = 10;

    private IPlayerMovement _activeMovement;

    void FixedUpdate()
    {
        _activeMovement.Move();
    }

    void Update()
    {
        _activeMovement.GetInput();
    }
}
 */