using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginCameraMove : MonoBehaviour
{
    public float speed = 0.2f; // ī�޶� �̵� �ӵ�
    public float distance = 5f; // �¿�� �̵��� �Ÿ�

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // �¿�� �̵��� �Ÿ� ���
        float offsetX = Mathf.Sin(Time.time * speed) * distance;

        // ���ο� ��ġ ���
        Vector3 newPosition = startPosition + new Vector3(offsetX, 0f, 0f);

        // ī�޶� �̵�
        transform.position = newPosition;
    }
}
