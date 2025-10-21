using System;

namespace CodeBase.Interfaces.Infrastructure.Services.UI
{
    public interface IMobileInput
    {
        float Movement { get;}
        float Rotation { get; }
        bool Attack { get; }
        bool Skill { get; }
        public event Action Destroyed;
        void Show();
        void Hide();
    }
}