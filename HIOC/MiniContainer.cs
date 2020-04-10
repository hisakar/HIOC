using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HIOC
{
    public class MiniContainer
    {
        private readonly Dictionary<Type, Type> _singletonTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> _scopedTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> _transientTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> _singletonObjects = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> _scopedObjects = new Dictionary<Type, object>();

        private static MiniContainer _miniContainer;

        private MiniContainer() { }

        public static MiniContainer GetInstance()
        {
            return _miniContainer = _miniContainer ?? new MiniContainer();
        }

        public void RegisterAsSingleton<TInterface, TImplementation>()
                   where TImplementation : TInterface
        {
            CheckRegisteredBefore(typeof(TInterface));

            _singletonTypes[typeof(TInterface)] = typeof(TImplementation);
            var instance = Resolve(typeof(TInterface));
            _singletonObjects[typeof(TInterface)] = instance;
        }

        public void RegisterAsScoped<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            CheckRegisteredBefore(typeof(TInterface));

            _scopedTypes[typeof(TInterface)] = typeof(TImplementation);
        }

        public void RegisterAsTransient<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            CheckRegisteredBefore(typeof(TInterface));

            _transientTypes[typeof(TInterface)] = typeof(TImplementation);
        }

        private void CheckRegisteredBefore(Type type)
        {
            if (_singletonTypes.ContainsKey(type) || _transientTypes.ContainsKey(type) ||
                _scopedTypes.ContainsKey(type))
            {
                throw new InvalidOperationException("The registry only can be registered once.");
            }
        }

        public TInterface Resolve<TInterface>()
        {
            Type type = typeof(TInterface);
            if (_singletonTypes.ContainsKey(type))
            {
                return (TInterface)ResolveAsSingleton(type);
            }
            if (_scopedTypes.ContainsKey(type))
            {
                return (TInterface)ResolveAsScoped(type);
            }
            if (_transientTypes.ContainsKey(type))
            {
                return (TInterface)ResolveAsTransient(type);
            }
            throw new InvalidOperationException("The registry is not recognized.Please ensure your registry is registered before try to resolve.");
        }

        private object Resolve(Type type)
        {
            if (_singletonTypes.ContainsKey(type))
            {
                return ResolveAsSingleton(type);
            }
            if (_scopedTypes.ContainsKey(type))
            {
                return ResolveAsScoped(type);
            }
            if (_transientTypes.ContainsKey(type))
            {
                return ResolveAsTransient(type);
            }
            throw new InvalidOperationException("The registry is not recognized.Please ensure your registry registered before try to resolve.");
        }

        private object ResolveAsTransient(Type type)
        {
            var concreteType = _transientTypes[type];
            var defaultCtor = concreteType.GetConstructors().First();
            var defaultParams = defaultCtor.GetParameters();
            var parameters = defaultParams.Select(param => Resolve(param.ParameterType)).ToArray();

            return defaultCtor.Invoke(parameters);
        }

        //TODO
        private object ResolveAsScoped(Type type)
        {
            throw new NotImplementedException();
        }

        private object ResolveAsSingleton(Type type)
        {
            if (CheckInstanceExistInSingleton(type))
            {
                return _singletonObjects[type];
            }

            var concreteType = _singletonTypes[type];
            var defaultCtor = concreteType.GetConstructors().First();
            var defaultParams = defaultCtor.GetParameters();
            var parameters = defaultParams.Select(param => Resolve(param.ParameterType)).ToArray();

            return defaultCtor.Invoke(parameters);
        }

        private bool CheckInstanceExistInSingleton(Type type)
        {
            return _singletonObjects.ContainsKey(type);
        }
    }
}
