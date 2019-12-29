using System;
using src.element.effector;

namespace src.setting {
    public class Setting<T> : ISetting {

        private T _value;
        private Action<T> _onUpdated;
        private readonly ArgumentParser.TryParseDelegate<T> _parser;
        
        private Action _onGenericUpdated;
        
        public Setting(string startValue, ArgumentParser.TryParseDelegate<T> parse) {
            _parser = parse;
            update(startValue);
        }

        public void update(string newValue) {
            _parser.Invoke(newValue, out _value);
            _onUpdated?.Invoke(_value);
            _onGenericUpdated?.Invoke();
        }

        public void getValue(Action<T> callback) {
            _onUpdated += callback;
            callback.Invoke(_value);
        }
        
        public T getValue() {
            return _value;
        }

        public string getValuesAsString() {
            return _value.ToString();
        }

        public void addOnUpdatedCallback(Action callback) {
            _onGenericUpdated += callback;
        }
    }
}