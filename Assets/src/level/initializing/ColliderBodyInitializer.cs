using System;
using System.Collections.Generic;
using System.Reflection;
using src.element;
using src.element.collider_body;
using src.element.effector;
using UnityEngine;

namespace src.level.initializing {
    
    /// <summary>
    /// Initializes a new Effector
    /// </summary>
    public class ColliderBodyInitializer : ElementInitializer {
        
        private readonly Dictionary<string, string> _parameters;
        
        public ColliderBodyInitializer(Dictionary<string, string> parameters,
            ElementType elementType, Vector2 position, float angle)
            : base(elementType, position, angle) {
            _parameters = parameters;
        }

        protected override void callSetupScript(GameObject currentGameObject) {
            // Firstly get the effectors type
            var colliderBody = currentGameObject.GetComponent<ColliderBody>();
            Type colliderBodyType = colliderBody.GetType();
            
            // Then get its methods and prepare its to search for values
            var methods = colliderBodyType.GetMethods();
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
                    if (!_parameters.TryGetValue(parameterInfo.Name, out var currentParameter)) {
                        throw new Exception($"Could not find parameter {parameterInfo.Name}");
                    }
                    parameters[parameterInfo.Position] = currentParameter;
                }

                break;
            }

            // And finally invoke it with its parameters
            setupFunction.Invoke(colliderBody, parameters);
        }
    }
}