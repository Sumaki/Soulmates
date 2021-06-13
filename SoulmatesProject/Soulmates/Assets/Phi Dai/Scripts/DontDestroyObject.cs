using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    public GameObject obj;

    private void Awake()
    {
        DontDestroyOnLoad(obj);
    }
}
