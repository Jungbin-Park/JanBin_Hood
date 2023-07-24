using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2f;   

    public float max_posX = 5.5f;
    public float max_posY = 10.5f;
    bool ismove;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) ismove = true;
        if (Input.GetKeyUp(KeyCode.LeftAlt)) ismove = false;

        float mouseX = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition).x - 0.5f;
        float mouseY = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition).y - 0.5f;

        //movX = mouseX * sensitivity * Time.deltaTime;
        //movY = mouseY * sensitivity * Time.deltaTime;

        Vector3 position
            = new Vector3(mouseX, mouseY, 0f);

        Vector3 move = position * (Time.deltaTime * sensitivity);
        //Debug.Log(Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition)); 마우스월드좌표
        

        Vector3 pos = transform.position;
        if (ismove)
        {
            pos.x = Mathf.Clamp(pos.x, -max_posX, max_posX);
            pos.y = Mathf.Clamp(pos.y, -max_posY, max_posY);

            transform.position = pos;
            transform.Translate(move);
        }       

    }
}