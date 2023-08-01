using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    [SerializeField] private float spawnDistance = 3f; // Дистанция от камеры до точки спавна
    [SerializeField] private MonoBehaviour[] ignoredComponents; // Для фильтрования объектов сцены
    List<GameObject> objectPool = new List<GameObject>(); // Отфильтрованные объекты на сцене
    [SerializeField] private Material standartMaterial; // Для изменения прозрачности

    private void OnEnable()
    {
        // Находим все объекты на сцене (включая камеры и UI элементы)
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Фильтруем объекты, исключая камеры, UI элементы и объекты с компонентами из списка игнорируемых компонентов
        foreach (GameObject obj in allObjects)
        {
            if (!IsIgnoredObject(obj))
            {
                objectPool.Add(obj);
            }
        }
    }

    private bool IsIgnoredObject(GameObject obj)
    {
        // Проверяем, является ли объект UI элементом
        if (obj.layer == LayerMask.NameToLayer("UI"))
        {
            return true;
        }
        // Проверяем, является ли объект светом
        if (obj.GetComponent<Light>() != null)
        {
            return true;
        }
        // Проверяем, имеет ли объект хотя бы один из компонентов из списка игнорируемых компонентов
        foreach (MonoBehaviour component in ignoredComponents)
        {
            if (obj.GetComponent(component.GetType()) != null)
            {
                return true;
            }
        }
        return false;
    }

    public GameObject CreatePrimitiveObject(int primitiveId)
    {
        // Получаем позицию и направление камеры
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // Вычисляем позицию для создания нового объекта
        Vector3 spawnPosition = cameraPosition + cameraForward * spawnDistance;

        // Создаем объект и сохраняем ссылку в пул
        GameObject gameObject = GameObject.CreatePrimitive((PrimitiveType)primitiveId);
        gameObject.transform.position = spawnPosition;
        gameObject.GetComponent<Renderer>().material = standartMaterial;
        objectPool.Add(gameObject);

        return gameObject;
    }

    public void RemoveObject(GameObject gameObject)
    {
        // Удаление в пуле и со сцены
        objectPool.Remove(gameObject);
        Destroy(gameObject);
    }

    public List<GameObject> GetListObjects()
    {
        return objectPool;
    }
}
