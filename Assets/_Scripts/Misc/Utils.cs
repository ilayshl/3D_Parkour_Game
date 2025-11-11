public static class Utils
{
    //Clamps angles between -180 to 180 to prevent Unity shenanigans.
    public static float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
    }
}
