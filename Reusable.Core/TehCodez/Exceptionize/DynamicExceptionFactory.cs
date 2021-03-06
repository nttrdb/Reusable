using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using JetBrains.Annotations;

namespace Reusable.Exceptionize
{
    public interface IDynamicExceptionFactory
    {
        /// <summary>
        /// Creates a dynamic exception where the name should end with 'Exception'. In debug mode there is an Assert that will remind you of this.
        /// </summary>
        /// <param name="name">The name of the exception. It should end with 'Exception'. An Assert will remind you of this in DEBUG mode.</param>
        /// <param name="message">The message for the exception. It can be 'null' but you should provide it anyway if you want to find what wend wrong later</param>
        /// <param name="innerException">The inner exception. It can be 'null' but remember to set it if you have one.</param>
        [NotNull, ContractAnnotation("name: null => halt")]
        Exception CreateDynamicException([NotNull] string name, [CanBeNull] string message, [CanBeNull] Exception innerException);

        /// <summary>
        /// Gets a dynamic excepiton type.
        /// </summary>
        /// <param name="name"></param>
        Type GetDynamicExceptionType([NotNull] string name);
    }

    internal class DynamicExceptionFactory : IDynamicExceptionFactory
    {
        private readonly ConcurrentDictionary<string, Type> _cache = new ConcurrentDictionary<string, Type>();

        public static IDynamicExceptionFactory Default { get; } = new DynamicExceptionFactory();

        public Exception CreateDynamicException(string name, string message, Exception innerException)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var dynamicExceptionType = GetDynamicExceptionType(name);            
            return (Exception)Activator.CreateInstance(dynamicExceptionType, message, innerException);
        }

        public Type GetDynamicExceptionType(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _cache.GetOrAdd(name, CreateDynamicExceptionType);
        }

        private Type CreateDynamicExceptionType(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            Debug.Assert(name.EndsWith(nameof(Exception)), $"Exception name should end with '{nameof(Exception)}'.");

            var baseType = typeof(DynamicException);
            var baseConstructorParameterTypes = new[] { typeof(string), typeof(Exception) };
            var baseConstructor = baseType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, baseConstructorParameterTypes, null);

            var assemblyName = new AssemblyName($"DynamicAssembly_{Guid.NewGuid():N}");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
            var typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public);
            typeBuilder.SetParent(typeof(DynamicException));

            // Create a constructor with the same number of parameters as the base constructor.
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, baseConstructorParameterTypes);

            var ilGenerator = constructor.GetILGenerator();

            // Generate constructor code
            ilGenerator.Emit(OpCodes.Ldarg_0);                // push 'this' onto stack.
            ilGenerator.Emit(OpCodes.Ldarg_1);                // push 'message' onto stack.
            ilGenerator.Emit(OpCodes.Ldarg_2);                // push 'innerException' onto stack.
            ilGenerator.Emit(OpCodes.Call, baseConstructor);  // call base constructor

            ilGenerator.Emit(OpCodes.Nop);                    // C# compiler add 2 NOPS, so
            ilGenerator.Emit(OpCodes.Nop);                    // we'll add them, too.

            ilGenerator.Emit(OpCodes.Ret);                    // Return

            return typeBuilder.CreateType();
        }
    }
}