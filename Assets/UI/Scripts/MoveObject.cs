using Unity.VisualScripting;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // 회전 속도
    public float zoomSpeed = 0.01f;    // 확대/축소 속도
    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        // 회전기능
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // 터치 이동에 따른 회전
                float rotationX = touch.deltaPosition.x * rotationSpeed;
                float rotationY = touch.deltaPosition.y * rotationSpeed;

                // 오브젝트 회전
                transform.Rotate(Vector3.up, -rotationX, Space.World);
                transform.Rotate(Vector3.right, rotationY, Space.World);
            }
        }

        // 확대 축소 기능
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // 터치간 거리 계산
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
                initialScale = transform.localScale;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);

                if (Mathf.Approximately(initialDistance, 0))
                {
                    return;
                }

                // 거리 비율 계산
                float factor = currentDistance / initialDistance;

                // 오브젝트 크기 조정 (확대/축소)
                transform.localScale = initialScale * factor;
            }
        }
    }
}