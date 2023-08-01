using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target; // Объект, вокруг которого будет вращаться камера

    [SerializeField] private float rotationSpeed = 5f; // Скорость вращения камеры
    [SerializeField] private float zoomSpeed = 2f; // Скорость отдаления камеры
    [SerializeField] private float movementSpeed = 5.0f; // Скорость перемещения камеры

    [SerializeField] private float minZoomDistance = 1f; // Минимальное расстояние от камеры до объекта
    [SerializeField] private float maxZoomDistance = 10f; // Максимальное расстояние от камеры до объекта
    [SerializeField] private float zoomDistance = 5f; // Текущее расстояние от камеры до объекта

    private float mouseX, mouseY; // Ввод мыши
    private Vector3 negDistance; // Для расчета расстояния камеры от объекта
    private Vector3 position; // Для расчета позиции камеры

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            target = null;

        if (target == null)
        {
            // Перемещение камеры
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");


            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
            direction = transform.TransformDirection(direction);
            transform.position += direction * movementSpeed * Time.deltaTime;

            // Поворот камеры
            if (Input.GetMouseButton(1))
            {
                mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
                mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed; // Инвертируем для интуитивного управления

                transform.eulerAngles += new Vector3(mouseY, mouseX, 0);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                // Получаем перемещение мыши по осям
                mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
                mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            }
            // Ограничиваем угол вращения по вертикали (мышь вверх и вниз)
            mouseY = Mathf.Clamp(mouseY, -89f, 89f);

            // Поворачиваем камеру вокруг объекта
            Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
            negDistance = new Vector3(0.0f, 0.0f, -zoomDistance); // Расстояние камеры от объекта
            position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;

            // Изменяем расстояние камеры с помощью колесика мыши
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
