using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class LoadFromKeyAttribute : Attribute
{
    public string Key { get; }

    public LoadFromKeyAttribute(string key)
    {
        Key = key;
    }
}
