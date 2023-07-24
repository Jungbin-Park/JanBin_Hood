using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit2D hit2d;
        //RaycastHit hit;
        ////Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward,Color.magenta, 180f, true);
        //if (Physics.Raycast(ray, out hit2d))
        //{
        //    Debug.Log(hit.collider.name);
        //}
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit2d = Physics2D.Raycast(ray.origin,ray.direction);
        if (hit2d.collider != null)
        {
            Debug.Log(hit2d.collider.name);
        }
    }
}
