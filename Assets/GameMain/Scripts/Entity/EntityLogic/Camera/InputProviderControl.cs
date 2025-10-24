using Cinemachine;

namespace KSG
{
    public class InputProviderControl : CinemachineInputProvider
    {
        public void DisableInputProvider()
        {
            enabled = false;
            gameObject.SetActive(false);
        }

        public void EnableInputProvider()
        {
            enabled = true;
            gameObject.SetActive(true);
        }
    }
}