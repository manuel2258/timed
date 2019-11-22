using System;
using src.element.effector;

namespace src.setting {
    public class Setting<T> {

        private T value;
        private Action<T> onUpdated;
        private readonly ArgumentParser.TryParseDelegate<T> parser;
        
        public Setting(string startValue, ArgumentParser.TryParseDelegate<T> parse) {
            parser = parse;
            update(startValue);
        }

        public void update(string newValue) {
            parser.Invoke(newValue, out value);
            onUpdated?.Invoke(value);
        }

        public void getValue(Action<T> callback) {
            onUpdated += callback;
            callback.Invoke(value);
        }
    }
}