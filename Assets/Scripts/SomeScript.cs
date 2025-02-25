using UnityEngine;

public class SomeScript : MonoBehaviour
{
    [SerializeReference]
    public ReorderableListAbility list = new ReorderableListAbility();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Add Ice Ability")]
    public void AddIceAbility()
    {
        //abilities.Add(new AbilityIce()
        //{
        //    range = 10
        //});
    }

    [ContextMenu("Add Fire Ability")]
    public void AddFireAbility()
    {
        //abilities.Add(new AbilityFire()
        //{
        //    damage = 10
        //});
    }
}

[System.Serializable]
public abstract class CustomAbility
{
    public string name;
}

[System.Serializable]
public class AbilityFire : CustomAbility
{
    public float damage;
}

[System.Serializable]
public class AbilityIce : CustomAbility
{
    public bool isOP = true;
    public float range = 100;
}

[System.Serializable]
public class AbilityPoison : CustomAbility
{
    public float coolness = 3.0f;
    public float range = 100;
}