using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtension
{
    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        [SerializeField]
        private T value;
        public UnityEvent<T> OnValueChanged;

        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                value = Value;
                OnValueChanged?.Invoke(value);
            }
        }
    }
}