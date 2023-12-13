using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityCamera : MonoBehaviour
{
    public Transform character; // 캐릭터의 Transform을 할당합니다.
    private Renderer obstacleRenderer; // 장애물의 Renderer를 참조할 변수입니다.

    void Update()
    {
        // 캐릭터와 이 객체 사이의 거리를 계산합니다.
        float distance = Vector3.Distance(transform.position, character.position);

        // 이 객체에서 캐릭터 방향으로의 벡터를 계산합니다.
        Vector3 direction = (character.position - transform.position).normalized;

        RaycastHit hit;

        // 레이캐스트를 발사합니다.
        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            // 레이에 맞은 오브젝트에서 Renderer 컴포넌트를 가져옵니다.
            obstacleRenderer = hit.transform.GetComponentInChildren<Renderer>();

            if (obstacleRenderer != null)
            {
                // Renderer의 Material의 투명도를 조정합니다.
                Material mat = obstacleRenderer.material;
                Color matColor = mat.color;
                matColor.a = 0.5f; // 투명도를 50%로 설정합니다.
                mat.color = matColor;
            }
        }
    }
}
