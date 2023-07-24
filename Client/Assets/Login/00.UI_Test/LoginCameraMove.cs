using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginCameraMove : MonoBehaviour
{
    public float speed = 0.2f; // 카메라 이동 속도
    public float distance = 5f; // 좌우로 이동할 거리

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // 좌우로 이동할 거리 계산
        float offsetX = Mathf.Sin(Time.time * speed) * distance;

        // 새로운 위치 계산
        Vector3 newPosition = startPosition + new Vector3(offsetX, 0f, 0f);

        // 카메라 이동
        transform.position = newPosition;
    }
}
