using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMain : GameScene
{
    static public void GoMainScene()
    {
        SceneManager.LoadScene("Game");          
    }

    public override void Clear()
    {

    }
}
