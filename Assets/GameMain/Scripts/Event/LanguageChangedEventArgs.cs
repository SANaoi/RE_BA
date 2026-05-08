using GameFramework;
using GameFramework.Event;
using GameFramework.Localization;

namespace KSG
{
    public sealed class LanguageChangedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LanguageChangedEventArgs).GetHashCode();
        public override int Id => EventId;

        public Language NewLanguage
        {
            get;
            private set;
        }

        public LanguageChangedEventArgs()
        {
            NewLanguage = Language.Unspecified;
        }

        public static LanguageChangedEventArgs Create(Language language)
        {
            LanguageChangedEventArgs args = ReferencePool.Acquire<LanguageChangedEventArgs>();
            args.NewLanguage = language;
            return args;
        }

        public override void Clear()
        {
            NewLanguage = Language.Unspecified;
        }
    }
}
