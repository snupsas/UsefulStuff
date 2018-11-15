using System;
using System.Collections.Generic;
using System.Linq;
using TinyIoC;

namespace IoC
{
    public static class TinyIocFactory
    {
        private static TinyIoCContainer _container = TinyIoCContainer.Current;
        private static List<Type> _registeredTypes = new List<Type>();

        public static TinyIoCContainer.RegisterOptions RegisterBinding<T1, T2>()
            where T1 : class
            where T2 : class, T1
        {

            var result = _container.Register<T1, T2>();
            _registeredTypes.Add(typeof(T1));
            return result;
        }

        public static TinyIoCContainer.RegisterOptions RegisterBinding(Type type)
        {
            var result = _container.Register(type);
            _registeredTypes.Add(type);
            return result;
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        public static void BuildUp(object input)
        {
            var properties = from property in input.GetType().GetProperties()
                where _registeredTypes.Any(x => x == property.PropertyType)
                select property;

            foreach (var property in properties)
            {
                var type = property.PropertyType;
                var canResolve = _container.CanResolve(type);
                if (canResolve && property.GetValue(input, null) == null)
                {
                    try
                    {
                        property.SetValue(input, _container.Resolve(type));

                    }
                    catch (Exception ex)
                    {
                        // Catch any resolution errors and ignore them
                    }
                }
            }
        }
    }
}

// Use example
public abstract class SomeBaseClass
{
	protected SomeBaseClass()
	{
		TinyIocFactory.BuildUp(this);
	}
}

public class SomeClass : SomeBaseClass
{
	public SomeRegisteredServiceThatWillBeAutomaticallyResolved {get;set;}
}