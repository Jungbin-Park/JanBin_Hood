using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public List<GameObject> builded = new List<GameObject>();
    public int turn = 1;
    static public int maxturn = 2500;
    float time = 0;
    public delegate void CheckHarvDele();
    public CheckHarvDele harvDele;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        time += Time.deltaTime;
        if(time > 1f)
        {
            turn++;
            time= 0;
            if(harvDele!= null) 
                harvDele();
            if (turn > maxturn)
            {
                turn = 1;
            }
        }
    }
}
