namespace KSG
{
    public static partial class PlayerConstantData
    {
        public static class CrosshairData
        {
            public const float INDEX = 0.8f;
            public const float RESTINGSIZE = 100f * INDEX;
            public const float MAXSIZE = 200f * INDEX;
            public const float SPEED = 10f * INDEX;
            public const float AIMINGSPREAD = -30f * INDEX;
            public const float WALKINGSPREAD = 40f * INDEX;
            public const float RUNNINGSPREAD = 60f * INDEX;
            public const float SHOOTINGSPREAD = 40f * INDEX;
            public const int TargetLayerMask = 1 << 8;
        }
    }
}