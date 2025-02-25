using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SomeScript))]
public class CustomAbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //SomeScript someScript = (SomeScript)target;
        //if(GUILayout.Button("Add Fire Ability"))
        //{
        //    someScript.AddFireAbility();
        //}

        //if (GUILayout.Button("Add Ice Ability"))
        //{
        //    someScript.AddIceAbility();
        //}
    }
}
