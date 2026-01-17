using System.Collections;
using UnityEngine;

public class Hello
{
    public static void Run()
    {
        Debug.Log("Hello, HybridCLR");
        GameObject go = new GameObject("Test1");
        go.AddComponent<Print>();
    }
}