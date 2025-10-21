using CodeBase.Interfaces.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.AnalyticsService
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        private FirebaseApp _app;
        private bool _isInitialized;

        public async UniTask Initialize()
        {
#if UNITY_EDITOR
            Debug.Log("Firebase Analytics would be initialized in build");
            _isInitialized = true;
#else
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            
            if (dependencyStatus == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                _isInitialized = true;
                Debug.Log("Firebase Analytics initialized successfully");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                _isInitialized = false;
            }
#endif
        }

        public UniTask SendEvent(string eventName)
        {
            if (!_isInitialized) return UniTask.CompletedTask;
            
            FirebaseAnalytics.LogEvent(eventName);
            return UniTask.CompletedTask;
        }

        public UniTask SendEvent(string eventName, Dictionary<string, object> parameters)
        {
            if (!_isInitialized) return UniTask.CompletedTask;

            var parameterArray = new Parameter[parameters.Count];
            var index = 0;
            
            foreach (var param in parameters)
            {
                parameterArray[index] = ConvertToFirebaseParameter(param.Key, param.Value);
                index++;
            }
            
            FirebaseAnalytics.LogEvent(eventName, parameterArray);
            return UniTask.CompletedTask;
        }

        private Parameter ConvertToFirebaseParameter(string key, object value)
        {
            switch (value)
            {
                case int intValue:
                    return new Parameter(key, intValue);
                case long longValue:
                    return new Parameter(key, longValue);
                case double doubleValue:
                    return new Parameter(key, doubleValue);
                case float floatValue:
                    return new Parameter(key, floatValue);
                default:
                    return new Parameter(key, value.ToString());
            }
        }
    }
}