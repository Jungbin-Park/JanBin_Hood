using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{

    public void TestJson()
    {
        Vector2 e = Vector2.one;
        string s = JsonConvert.SerializeObject(e);
        Debug.Log(s);
    }
}
