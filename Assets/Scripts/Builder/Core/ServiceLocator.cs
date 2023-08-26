using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Builder.Core
{
    /// <summary>
    /// サービスロケーター
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator instance;

        public static ServiceLocator Instance => instance;

        private static readonly Dictionary<Type, object> InstanceDict = new Dictionary<Type, object>();

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// 単一インスタンスを登録する
        /// 呼び直すと上書き登録する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="instance">インスタンス</param>
        public void Register<T>(T instance) where T : class
        {
            if (instance == null)
            {
                Debug.LogWarning("ServiceLocator: nullのインスタンスを登録しています!");
            }

            InstanceDict[typeof(T)] = instance;
        }

        /// <summary>
        /// 型を指定して登録されているインスタンスを取得する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <returns>インスタンス</returns>
        public T Resolve<T>() where T : class
        {
            Type type = typeof(T);

            if (InstanceDict.TryGetValue(type, out object value))
            {
                T instance = value as T;
                return instance;
            }

            Debug.LogWarning($"ServiceLocator: {typeof(T).Name} クラスは存在しません");
            return null;
        }
    }
}