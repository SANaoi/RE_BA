using GameFramework;
using GameFramework.Event;

namespace KSG
{
    public class PlayerCrosshairChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerCrosshairChangeEventArgs).GetHashCode();
        public override int Id => EventId;

        public float CrosshairSize
        {
            get;
            private set;
        }

        public PlayerCrosshairChangeEventArgs()
        {
            CrosshairSize = 0;
        }

        public static PlayerCrosshairChangeEventArgs Create(float crosshairSize)
        {
            PlayerCrosshairChangeEventArgs args = ReferencePool.Acquire<PlayerCrosshairChangeEventArgs>();
            args.CrosshairSize = crosshairSize;
            return args;
        }

        public override void Clear()
        {
            CrosshairSize = 0;
        }
    }
}