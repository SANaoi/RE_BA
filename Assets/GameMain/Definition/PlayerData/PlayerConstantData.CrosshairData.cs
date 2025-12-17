namespace KSG
{
    public static partial class PlayerConstantData
    {
        public static class CrosshairData
        {
            public const float RESTINGSIZE = 100f;
            public const float MAXSIZE = 200f;
            public const float SPEED = 10f;
            public const float AIMINGSPREAD = -30f;
            public const float WALKINGSPREAD = 40f;
            public const float RUNNINGSPREAD = 60f;
            public const float SHOOTINGSPREAD = 40f;
            public const int TargetLayerMask = 1 << 8;
        }
    }
}