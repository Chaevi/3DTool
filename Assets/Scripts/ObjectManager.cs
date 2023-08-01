using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    [SerializeField] private float spawnDistance = 3f; // ��������� �� ������ �� ����� ������
    [SerializeField] private MonoBehaviour[] ignoredComponents; // ��� ������������ �������� �����
    List<GameObject> objectPool = new List<GameObject>(); // ��������������� ������� �� �����
    [SerializeField] private Material standartMaterial; // ��� ��������� ������������

    private void OnEnable()
    {
        // ������� ��� ������� �� ����� (������� ������ � UI ��������)
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // ��������� �������, �������� ������, UI �������� � ������� � ������������ �� ������ ������������ �����������
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
        // ���������, �������� �� ������ UI ���������
        if (obj.layer == LayerMask.NameToLayer("UI"))
        {
            return true;
        }
        // ���������, �������� �� ������ ������
        if (obj.GetComponent<Light>() != null)
        {
            return true;
        }
        // ���������, ����� �� ������ ���� �� ���� �� ����������� �� ������ ������������ �����������
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
        // �������� ������� � ����������� ������
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // ��������� ������� ��� �������� ������ �������
        Vector3 spawnPosition = cameraPosition + cameraForward * spawnDistance;

        // ������� ������ � ��������� ������ � ���
        GameObject gameObject = GameObject.CreatePrimitive((PrimitiveType)primitiveId);
        gameObject.transform.position = spawnPosition;
        gameObject.GetComponent<Renderer>().material = standartMaterial;
        objectPool.Add(gameObject);

        return gameObject;
    }

    public void RemoveObject(GameObject gameObject)
    {
        // �������� � ���� � �� �����
        objectPool.Remove(gameObject);
        Destroy(gameObject);
    }

    public List<GameObject> GetListObjects()
    {
        return objectPool;
    }
}
