using System;

namespace src.element.effector {
    public class ArgumentParser {

        private readonly string _effectorName;

        public ArgumentParser(string effectorName) {
            _effectorName = effectorName;
        }

        public delegate bool TryParseDelegate<T>(string str, out T value);

        public T TryParse<T>(string argument, TryParseDelegate<T> parse) {
            if(!parse(argument, out T value)) {
                throw new Exception($"{_effectorName}: Could not parse argument -> " + argument);
            }

            return value;
        }
    }
}