
public struct PlayerMovementData
{
    public float Damp;
    public float MoveSpeedMult;
    public float SpeedLimitMult;

    public PlayerMovementData(float damp, float moveSpeedMult, float speedLimitMult)
    {
        Damp = damp;
        MoveSpeedMult = moveSpeedMult;
        SpeedLimitMult = speedLimitMult;
    }
}
