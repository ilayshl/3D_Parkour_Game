public static class Utils
{
    //Clamps angles between -180 to 180.
   public static float NormalizeAngle(float angle)
     {
        angle %= 360f;
        if(angle > 180f)
        {
            angle -= 360f;
        }
            return angle;
    }
}
