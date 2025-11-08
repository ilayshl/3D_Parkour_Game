public interface IPlayerManager
{
    public void HandleMove();
    public void SetMovementData(PlayerMovementData data);
    public void HandleJump();
    public void HandleSwingMove();
    public void HandleSwingStop();
}
