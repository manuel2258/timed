using System;
using System.Collections.Generic;
using System.Reflection;

namespace src.level.initializing {
    public static class InitializeHelper {
        
        public static void initializeObject(ISetupAble initializeAble, Dictionary<string, string> nameParameterMap) {
            Type type = initializeAble.GetType();
            // Then get its methods and prepare its to search for values
            var methods = type.GetMethods();
            object[] parameters = new object[0];
            MethodInfo setupFunction = methods[0];
            foreach (var methodInfo in methods) {
                // Check if the method is named setup
                if(methodInfo.Name != "setup") continue;
                
                // If so saves the method for later
                setupFunction = methodInfo;
                
                // Then get its parameter and initialize the parameter buffer
                var parameterInfos = methodInfo.GetParameters();
                parameters = new object[parameterInfos.Length];
                foreach (var parameterInfo in parameterInfos) {
                    // Then maps its values and fill the buffer
                    if (!nameParameterMap.TryGetValue(parameterInfo.Name, out var currentParameter)) {
                        throw new Exception($"Could not find parameter {parameterInfo.Name}");
                    }
                    parameters[parameterInfo.Position] = currentParameter;
                }

                break;
            }
            // And finally invoke it with its parameters
            setupFunction.Invoke(initializeAble, parameters);
        }
    }
    
    public interface ISetupAble { }
}