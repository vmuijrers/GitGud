using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ResourceCollection : ScriptableObject
{
    public List<Object> favorites = new List<Object>();
}