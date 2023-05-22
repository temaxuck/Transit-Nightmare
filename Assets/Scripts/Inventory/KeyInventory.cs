using UnityEngine;
using System.Collections.Generic;

public class KeyInventory : MonoBehaviour
{
    public static KeyInventory instance;

    public List<GameObject> keys;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one KeyInventory instance found!");
            return;
        }
        instance = this;
        keys = new List<GameObject>();
    }

    public void AddKey(GameObject key)
    {
        keys.Add(key);
    }

    public bool HasKey(GameObject key)
    {
        return keys.Contains(key);
    }
}
