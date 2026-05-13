using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KSG
{
    public static class UIWindowController
    {
        private static int s_LastTickFrame = -1;

        private sealed class ManagedFormState
        {
            public bool LockGameplayInput;
            public bool ShowCursor;
            public bool CloseByEscape;
        }

        private static readonly Dictionary<EnumUIForm, ManagedFormState> s_ManagedForms = new Dictionary<EnumUIForm, ManagedFormState>();
        private static readonly List<EnumUIForm> s_OpenOrder = new List<EnumUIForm>();

        public static bool IsGameplayInputLocked { get; private set; }

        public static bool ToggleForm(EnumUIForm formId, object userData = null)
        {
            if (GameEntry.UI.HasUIForm(formId))
            {
                return CloseForm(formId);
            }

            return GameEntry.UI.OpenUIForm(formId, userData).HasValue;
        }

        public static bool CloseForm(EnumUIForm formId)
        {
            UGuiForm uiForm = GameEntry.UI.GetUIForm(formId);
            if (uiForm == null)
            {
                UnregisterForm(formId);
                return false;
            }

            uiForm.Close(true);
            return true;
        }

        public static void RegisterForm(EnumUIForm formId, bool lockGameplayInput, bool showCursor, bool closeByEscape)
        {
            ManagedFormState state;
            if (!s_ManagedForms.TryGetValue(formId, out state))
            {
                state = new ManagedFormState();
                s_ManagedForms.Add(formId, state);
            }

            state.LockGameplayInput = lockGameplayInput;
            state.ShowCursor = showCursor;
            state.CloseByEscape = closeByEscape;

            if (!s_OpenOrder.Contains(formId))
            {
                s_OpenOrder.Add(formId);
            }

            RefreshGlobalState();
        }

        public static void UnregisterForm(EnumUIForm formId)
        {
            s_ManagedForms.Remove(formId);
            s_OpenOrder.Remove(formId);
            RefreshGlobalState();
        }

        public static void Tick()
        {
            if (s_LastTickFrame == Time.frameCount)
            {
                return;
            }

            s_LastTickFrame = Time.frameCount;

            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                TryCloseTopmostEscapeForm();
            }
        }

        private static bool TryCloseTopmostEscapeForm()
        {
            for (int i = s_OpenOrder.Count - 1; i >= 0; i--)
            {
                EnumUIForm formId = s_OpenOrder[i];
                ManagedFormState state;
                if (!s_ManagedForms.TryGetValue(formId, out state))
                {
                    continue;
                }

                if (!state.CloseByEscape)
                {
                    continue;
                }

                return CloseForm(formId);
            }

            return false;
        }

        private static void RefreshGlobalState()
        {
            bool lockGameplayInput = false;
            bool showCursor = false;

            foreach (EnumUIForm formId in s_OpenOrder)
            {
                ManagedFormState state;
                if (!s_ManagedForms.TryGetValue(formId, out state))
                {
                    continue;
                }

                lockGameplayInput |= state.LockGameplayInput;
                showCursor |= state.ShowCursor;
            }

            IsGameplayInputLocked = lockGameplayInput;
            Cursor.visible = showCursor;
            Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
