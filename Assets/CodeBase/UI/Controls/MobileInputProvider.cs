using CodeBase.Interfaces.Infrastructure.Services.UI;

namespace CodeBase.UI.Controls
{
    public class MobileInputProvider : IMobileInputProvider
    {
        public IMobileInput MobileInput { get; private set; }
        public bool Available { get; private set; }

        public void Register(IMobileInput mobileInput)
        {
            MobileInput = mobileInput;
            Available = true;
            MobileInput.Destroyed += MobileInputOnDestroyed;
        }

        private void MobileInputOnDestroyed()
        {
            Available = false;
        }
    }
}