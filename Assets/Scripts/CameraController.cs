using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target; // ������, ������ �������� ����� ��������� ������

    [SerializeField] private float rotationSpeed = 5f; // �������� �������� ������
    [SerializeField] private float zoomSpeed = 2f; // �������� ��������� ������
    [SerializeField] private float movementSpeed = 5.0f; // �������� ����������� ������

    [SerializeField] private float minZoomDistance = 1f; // ����������� ���������� �� ������ �� �������
    [SerializeField] private float maxZoomDistance = 10f; // ������������ ���������� �� ������ �� �������
    [SerializeField] private float zoomDistance = 5f; // ������� ���������� �� ������ �� �������

    private float mouseX, mouseY; // ���� ����
    private Vector3 negDistance; // ��� ������� ���������� ������ �� �������
    private Vector3 position; // ��� ������� ������� ������

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            target = null;

        if (target == null)
        {
            // ����������� ������
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");


            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
            direction = transform.TransformDirection(direction);
            transform.position += direction * movementSpeed * Time.deltaTime;

            // ������� ������
            if (Input.GetMouseButton(1))
            {
                mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
                mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed; // ����������� ��� ������������ ����������

                transform.eulerAngles += new Vector3(mouseY, mouseX, 0);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                // �������� ����������� ���� �� ����
                mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
                mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            }
            // ������������ ���� �������� �� ��������� (���� ����� � ����)
            mouseY = Mathf.Clamp(mouseY, -89f, 89f);

            // ������������ ������ ������ �������
            Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
            negDistance = new Vector3(0.0f, 0.0f, -zoomDistance); // ���������� ������ �� �������
            position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;

            // �������� ���������� ������ � ������� �������� ����
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            zoomDistance -= scrollWheel * zoomSpeed;
            zoomDistance = Mathf.Clamp(zoomDistance, minZoomDistance, maxZoomDistance);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
