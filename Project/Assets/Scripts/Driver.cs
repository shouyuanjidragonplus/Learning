using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    public static bool isWhiteListDevice = false;
    void Start()
    {
        SRDebug.Init();
        Debug.Log("Driver script started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
