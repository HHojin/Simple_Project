using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private Vector3 mousePos;
    public float moveSpeed = 20.0f;
    public float zoomSpeed = 10.0f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        mousePos = mainCamera.ScreenToViewportPoint(Input.mousePosition); // Viewport Point

        if(Input.GetKey(KeyCode.W) || mousePos.y >= 0.99f) // 위로 카메라 이동
        {
            //Debug.Log("Press W");
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.up, Space.World);
        }
        if (Input.GetKey(KeyCode.S) || mousePos.y <= 0.01f) // 아래로 카메라 이동
        {
            //Debug.Log("Press S");
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.down, Space.World);
        }
        if (Input.GetKey(KeyCode.A) || mousePos.x <= 0.01f) // 왼쪽으로 카메라 이동
        {
            //Debug.Log("Press A");
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.left, Space.World);
        }
        if (Input.GetKey(KeyCode.D) || mousePos.x >= 0.99f) // 오른쪽으로 카메라 이동
        {
            //Debug.Log("Press D");
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.right, Space.World);
        }
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            mainCamera.fieldOfView += distance;
        }
    }
}
