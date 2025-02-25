using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnumObjectContainer_", menuName = "SO/Enums/EnumObjectContainer")]
public class EnumObjectContainer : ScriptableObject
{
    public List<EnumObject> enumList = new List<EnumObject>();

}
