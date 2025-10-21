using System;
using CodeBase.UI.Binders.Main;
using TMPro;
using UniRx;

namespace CodeBase.UI.Binders
{
    public class TextBinder : IBinder, IObserver<string>
    {
        private readonly TextMeshProUGUI _textMeshPro;
        private readonly IReadOnlyReactiveProperty<string> _reactiveProperty;
        private IDisposable _handle;

        public TextBinder(TextMeshProUGUI textMeshPro, IReadOnlyReactiveProperty<string> reactiveProperty)
        {
            _textMeshPro = textMeshPro;
            _reactiveProperty = reactiveProperty;
        }
        public void Bind()
        {
            this.OnNext(_reactiveProperty.Value);
            _handle = _reactiveProperty.Subscribe(this);
        }

        public void Unbind()
        {
            _handle?.Dispose();
            _handle = null;
        }

        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(string value)
        {
            _textMeshPro.text = value;
        }
    }
}
