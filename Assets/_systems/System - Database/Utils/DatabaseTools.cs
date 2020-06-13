using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IScriptableIdentifiable<E>
    where E : System.Enum
{
    E EnumIdentifier { get; }
}

public interface IAssemblyIdentifiable<E>
    where E : System.Enum
{
    E EnumIdentifier { get; set; }
}

public interface IAssemblyScript<TParent>
{
    Type SourceClass { get; set; } // Subclass of TParent
}

public static class DatabaseTools
{
    /// <summary>
    /// Updates a list of serialized elements by tracking all subclasses of a type in the assembly.
    /// In this process, elements may be removed, added or updated to match existing types.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TWrapper"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    /// <param name="serializedData"></param>
    public static void RefreshAssemblyData<TKey, TParent>(ref List<Type> serializedData)
        where TKey : System.Enum
    {
        AssemblyDatabase<TKey, Type>.RefreshData(ref serializedData);
    }
    /// <summary>
    /// Updates a list of serialized elements by tracking all subclasses of a type in the assembly.
    /// Elements may be wrapped in a class of interface IAssemblyScript
    /// In this process, elements may be removed, added or updated to match existing types.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TWrapper"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    /// <param name="serializedData"></param>
    public static void RefreshAssemblyData<TKey, TWrapper, TParent>(ref List<TWrapper> serializedData)
        where TWrapper : IAssemblyScript<TParent>, IAssemblyIdentifiable<TKey>
        where TKey : System.Enum
    {
        AssemblyWrapperDatabase<TKey, TWrapper, TParent>.RefreshData(ref serializedData);
    }
    /// <summary>
    /// Updates a list of serialized elements by tracking all Scriptable Objects in Assets folder.
    /// In this process, elements may be removed, added or updated to match existing assets.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TWrapper"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    /// <param name="serializedData"></param>
    public static void RefreshScriptableData<TKey, TValue>(ref List<TValue> serializedData)
        where TValue : UnityEngine.ScriptableObject, IScriptableIdentifiable<TKey>
        where TKey : System.Enum
    {
        ScriptableObjectDatabase<TKey, TValue>.RefreshData(ref serializedData);
    }


    private class AssemblyWrapperDatabase<TKey, TWrapper, TParent> : Database<TKey, TWrapper, AssemblyWrapperDatabase<TKey, TWrapper, TParent>>
        where TWrapper : IAssemblyScript<TParent>, IAssemblyIdentifiable<TKey>
        where TKey : System.Enum
    {
        protected override TWrapper ValueMerge(TWrapper source, TWrapper original)
        {
            original.SourceClass = source.SourceClass;
            return original;
        }

        protected override bool EqualityComparer(TWrapper source, TWrapper original)
        {
            return source.SourceClass.Equals(original.SourceClass);
        }

        protected override Dictionary<TKey, TWrapper> GetDataSource()
        {
            Dictionary<TKey, Type> types = AssemblyDatabase<TKey, TParent>.GetAssemblyDataSource();

            // Map types to wrapper by creating an instance of wrapper for each type
            Dictionary<TKey, TWrapper> instances = new Dictionary<TKey, TWrapper>();
            foreach (var x in types)
            {
                TWrapper wrapper = Activator.CreateInstance<TWrapper>();
                wrapper.SourceClass = x.Value;
                wrapper.EnumIdentifier = x.Key;
                instances.Add(x.Key, wrapper);
            }
            return instances;
        }

        protected override Dictionary<TKey, TWrapper> SerializedDataAsDict(IEnumerable<TWrapper> serializedData)
        {
            return serializedData.ToDictionaryUnique(x => x.EnumIdentifier);
        }
    }    
    
    private class AssemblyDatabase<TKey, TParent> : Database<TKey, Type, AssemblyDatabase<TKey, TParent>>
        where TKey : System.Enum
    {
        public static Dictionary<TKey, Type> GetAssemblyDataSource()
        {
            // Get all child classes from assembly
            IEnumerable<Type> subclasses = typeof(TParent).GetSubclasses();
            return AssemblyTools.BuildDictionaryFromEnumProperty<TKey>(subclasses);
        }
        
        protected override Dictionary<TKey, Type> GetDataSource()
        {
            return GetAssemblyDataSource();
        }

        protected override Dictionary<TKey, Type> SerializedDataAsDict(IEnumerable<Type> serializedData)
        {
            return AssemblyTools.BuildDictionaryFromEnumProperty<TKey>(serializedData);
        }
    }

    private class ScriptableObjectDatabase<TKey, TValue> : Database<TKey, TValue, ScriptableObjectDatabase<TKey, TValue>>
        where TValue : UnityEngine.ScriptableObject, IScriptableIdentifiable<TKey>
        where TKey : System.Enum
    {
        protected override Dictionary<TKey, TValue> GetDataSource()
        {
            // Get All Scriptable Objects in Assets folder
            TValue[] scriptableObjects = Tools.GetAllScriptableObjects<TValue>();
            return scriptableObjects.ToDictionaryUnique(x => x.EnumIdentifier);
        }

        protected override Dictionary<TKey, TValue> SerializedDataAsDict(IEnumerable<TValue> serializedData)
        {
            // Return unique elements
            return serializedData.ToDictionaryUnique(x => x.EnumIdentifier);
        }
    }

    private abstract class Database<TKey, TValue, TDB> where TDB : Database<TKey, TValue, TDB>
    {
        protected abstract Dictionary<TKey, TValue> GetDataSource();

        protected abstract Dictionary<TKey, TValue> SerializedDataAsDict(IEnumerable<TValue> serializedData);

        public static void RefreshData(ref List<TValue> serializedData)
        {
            TDB instance = Activator.CreateInstance<TDB>();
            Dictionary<TKey, TValue> source = instance.GetDataSource();
            Dictionary<TKey, TValue> original = instance.SerializedDataAsDict(serializedData);
            serializedData = instance.RefreshData(original, source).Select(x => x.Value).ToList();
        }

        protected virtual TValue ValueMerge(TValue source, TValue original)
        {
            return source;
        }

        protected virtual bool EqualityComparer(TValue source, TValue original)
        {
            return source.Equals(original);
        }
        /**
         * Modifies the original to contain only elements of the source.
         * T ValueMerge(T original, T source): a function to merge two elements when source has been modified.
         */
        protected Dictionary<TKey, TValue> RefreshData(Dictionary<TKey, TValue> original, Dictionary<TKey, TValue> source)
        {
            // Refresh all serialized values which have been updated at source
            TKey[] intersection = original.Keys.Intersect(source.Keys).ToArray();
            foreach (var key in intersection)
            {
                if (!EqualityComparer(source[key], original[key]))
                {
                    original[key] = ValueMerge(source[key], original[key]);
                    Debug.LogWarning("Resetting Element from Database [" + key + " | " +
                        original[key] + "]");
                }
            }

            // Remove dest values which have been deleted from source
            var keys = original.Keys;
            foreach (var k in keys)
            {
                if (!source.ContainsKey(k))
                {
                    original.Remove(k);
                    Debug.LogWarning("Removing Element from Database [" + k + " | " +
                        original[k] + "]");
                }
            }
            // Add to dest values which are not already in it
            foreach (var s in source)
            {
                if (!original.ContainsKey(s.Key))
                {
                    original.Add(s.Key, s.Value);
                }
            }
            return original;
        }
    }
}