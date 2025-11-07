public interface IPlayerManager
{
    public void HandleMove();
    public void SetMovementData(PlayerMovementData data);
    public void HandleJump();
    public void HandleSwing();
}
