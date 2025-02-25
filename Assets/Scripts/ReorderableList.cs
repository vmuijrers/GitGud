using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReorderableList<T>
{
    [SerializeReference]
    public List<T> list = new List<T>();
}
[System.Serializable]
public class ReorderableListAbility : ReorderableList<CustomAbility>
{
}
