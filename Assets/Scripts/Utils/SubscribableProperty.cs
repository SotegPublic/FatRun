using System;

namespace Runner.Core
{
    public class SubscribableProperty<T>
    {
        protected Action<T> _onPropertyChange;
        protected T _value;

        public T Value => _value;

        protected void SetValue(T value)
        {
            _value = value;
            _onPropertyChange?.Invoke(_value);
        }

        public void Subscribe(Action<T> subscribingAction)
        {
            _onPropertyChange += subscribingAction;
        }

        public void Unsubscribe(Action<T> unsubscribingAction)
        {
            _onPropertyChange -= unsubscribingAction;
        }
    }
}