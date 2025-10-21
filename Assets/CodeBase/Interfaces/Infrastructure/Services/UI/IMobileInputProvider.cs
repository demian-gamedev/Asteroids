namespace CodeBase.Interfaces.Infrastructure.Services.UI
{
    public interface IMobileInputProvider
    {
        public IMobileInput MobileInput { get; }
        bool Available { get; }
        public void Register(IMobileInput mobileInput);
    }
}