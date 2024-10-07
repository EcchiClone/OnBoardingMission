using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierachyCategory : MonoBehaviour
{
    public static Dictionary<string, GameObject> parentsDict = new Dictionary<string, GameObject>();
    [SerializeField] GameObject[] parentGameObjects;

    void Start()
    {
        foreach(GameObject gameObject in parentGameObjects)
        {
            parentsDict.Add(gameObject.name.Substring(1, gameObject.name.Length - 2), gameObject);
        }
    }

}
