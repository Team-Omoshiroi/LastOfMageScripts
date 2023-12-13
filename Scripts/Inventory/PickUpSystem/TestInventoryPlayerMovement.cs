using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventoryPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // 입력을 받아서 움직임을 결정
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 움직임 벡터를 계산
        Vector3 move = new Vector3(x, 0f, z) * moveSpeed * Time.deltaTime;

        // 플레이어 위치를 업데이트
        transform.Translate(move);
    }
}
