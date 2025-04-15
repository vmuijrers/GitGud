using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu]
public class ResourceCollection : ScriptableObject
{
    public List<ResourceList> resources = new List<ResourceList>();

    public ResourceList GetResourceListByCategory(string category)
    {
        return resources.Find(x=>x.name == category);
    }

    public List<string> GetCategories()
    {
        return resources.Select(x=>x.name).ToList();
    }

    public void AddCategory(string category)
    {
        resources.Add(new ResourceList()
        {
            name = category,
            favorites = new List<Object>()
        });
    }
}

[System.Serializable]
public class ResourceList
{
    public string name;
    public bool isFoldout = true;
    public List<Object> favorites = new List<Object>();
}