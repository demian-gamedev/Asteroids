using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Common.Attributes;
using CodeBase.Interfaces.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Binders.Main
{
    public class DataBinder : MonoBehaviour
    {
        private static readonly List<BinderInfo> BinderInfosCache;

        static DataBinder()
        {
            BinderInfosCache = new List<BinderInfo>();
            IEnumerable<Type> binderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                {
                    try { return assembly.GetTypes(); }
                    catch { return Type.EmptyTypes; }
                })
                .Where(type => typeof(IBinder).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            foreach (Type type in binderTypes)
            {
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                if (constructors.Length == 1)
                {
                    BinderInfosCache.Add(new BinderInfo
                    {
                        BinderType = type,
                        ConstructorParameters = constructors[0].GetParameters()
                    });
                }
            }
        }

        [SerializeField] private UnityEngine.Object _targetView;

        private DiContainer _container;
        private readonly Dictionary<string, List<IDisposable>> _bindings = new();
        private readonly Dictionary<Type, object> _viewModels = new();

        [Inject]
        public void Construct(DiContainer container) => _container = container;

        private void Start() => BindData();
        private void OnDestroy() => UnbindAll();

        private void BindData() => BindComponent(_targetView);

        private void BindComponent(UnityEngine.Object component)
        {
            Type componentType = component.GetType();
            ProcessFields(component, componentType);
            ProcessProperties(component, componentType);
        }

        private void ProcessFields(UnityEngine.Object component, Type componentType)
        {
            FieldInfo[] fields = componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                DataAttribute dataAttribute = field.GetCustomAttribute<DataAttribute>();
                if (dataAttribute != null)
                {
                    BindField(component, field, dataAttribute.Id);
                }
            }
        }

        private void ProcessProperties(UnityEngine.Object component, Type componentType)
        {
            PropertyInfo[] properties = componentType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                DataAttribute dataAttribute = property.GetCustomAttribute<DataAttribute>();
                if (dataAttribute != null)
                {
                    BindProperty(component, property, dataAttribute.Id);
                }
            }
        }

        private void BindField(UnityEngine.Object component, FieldInfo field, string dataId)
        {
            object fieldValue = field.GetValue(component);
            if (fieldValue == null) return;

            TryCreateBinding(fieldValue, dataId);
        }

        private void BindProperty(UnityEngine.Object component, PropertyInfo property, string dataId)
        {
            object propertyValue = property.GetValue(component);
            if (propertyValue == null) return;

            TryCreateBinding(propertyValue, dataId);
        }

        private void TryCreateBinding(object component, string dataId)
        {
            Type componentType = component.GetType();
            Type viewModelType = FindViewModelTypeWithDataId(dataId, componentType);
            if (viewModelType == null) return;

            object viewModel = ResolveViewModel(viewModelType);
            object dataProperty = FindDataProperty(viewModel, dataId);
            if (dataProperty != null)
            {
                CreateBinding(component, dataProperty, dataId);
            }
        }

        private void CreateBinding(object component, object dataProperty, string dataId)
        {
            Type componentType = component.GetType();
            Type propertyType = dataProperty.GetType();

            BinderInfo suitableBinder = BinderInfosCache.FirstOrDefault(info =>
                info.ConstructorParameters.Length == 2 &&
                info.ConstructorParameters[0].ParameterType.IsAssignableFrom(componentType) &&
                info.ConstructorParameters[1].ParameterType.IsAssignableFrom(propertyType));

            if (suitableBinder == null)
            {
                Debug.LogError($"[DataBinder] No suitable IBinder found for types: ({componentType.Name}, {propertyType.Name})");
                return;
            }

            object[] constructorArgs = { component, dataProperty };
            IBinder binder = (IBinder)_container.Instantiate(suitableBinder.BinderType, constructorArgs);
            binder.Bind();

            if (!_bindings.ContainsKey(dataId))
                _bindings[dataId] = new List<IDisposable>();

            _bindings[dataId].Add(Disposable.Create(binder.Unbind));
        }

        private Type FindViewModelTypeWithDataId(string dataId, Type expectedType)
        {
            IEnumerable<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                {
                    try { return assembly.GetTypes(); }
                    catch { return Type.EmptyTypes; }
                });

            foreach (Type type in allTypes)
            {
                if (!typeof(IViewModel).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract)
                    continue;

                if (HasDataMember(type, dataId))
                    return type;
            }

            Debug.LogWarning($"ViewModel with data ID '{dataId}' not found");
            return null;
        }

        private bool HasDataMember(Type type, string dataId)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(property => property.GetCustomAttribute<DataAttribute>()?.Id == dataId)
                   || type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                       .Any(field => field.GetCustomAttribute<DataAttribute>()?.Id == dataId);
        }

        private object FindDataProperty(object viewModel, string dataId)
        {
            Type type = viewModel.GetType();

            PropertyInfo property = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(p => p.GetCustomAttribute<DataAttribute>()?.Id == dataId);
            if (property != null) return property.GetValue(viewModel);

            FieldInfo field = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.GetCustomAttribute<DataAttribute>()?.Id == dataId);
            return field?.GetValue(viewModel);
        }

        private object ResolveViewModel(Type viewModelType)
        {
            if (!_viewModels.TryGetValue(viewModelType, out object viewModel))
            {
                viewModel = _container.Resolve(viewModelType);
                _viewModels[viewModelType] = viewModel;
            }
            return viewModel;
        }

        private void UnbindAll()
        {
            foreach (List<IDisposable> bindingList in _bindings.Values)
            {
                foreach (IDisposable binding in bindingList)
                {
                    binding.Dispose();
                }
            }
            _bindings.Clear();
        }
    }

    internal class BinderInfo
    {
        public Type BinderType;
        public ParameterInfo[] ConstructorParameters;
    }
}