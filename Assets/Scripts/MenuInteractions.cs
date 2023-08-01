using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteractions : MonoBehaviour
{
    ObjectManager objectManager; // ��� ��������� ��������������� ��������

    public GameObject MenuElementPrefab; // ��� �������� �������� ���� ������� �����
    public ScrollRect scrollView; // ��������� ������� ��������

    private List<MenuElement> menuElements = new List<MenuElement>(); // ��� ���� ���������

    // �������� ��������� �����
    [SerializeField] private Slider SliderR;
    [SerializeField] private Slider SliderG;
    [SerializeField] private Slider SliderB;
    [SerializeField] private Slider SliderA;

    // �������� ��� ������������� �����
    [SerializeField] private Image CheckColorImage;

    private bool isShowed = false; // ����������� ����

    private void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
        GenerateScrollView();
    }

    public void ToggleMenu()
    {
        // ��������/���������� ����
        if (isShowed)
            isShowed = false;
        else
            isShowed = true;

        this.gameObject.SetActive(isShowed);
    }

    private void GenerateScrollView()
    {
        List<GameObject> sceneObjects = objectManager.GetListObjects();
        // ������� Scroll View ����� ��������� ����� ���������

        // �������� �� �������
        if (sceneObjects.Count < 1)
            return;

        ClearScrollView();

        // ������� UI �������� ��� ������� ������� �� ����
        foreach (GameObject obj in sceneObjects)
        {
            // ������� ��������� ������� UI ��������
            CreateNewMenuElement(obj);
        }
    }

    private void ClearScrollView()
    {
        // ������� ��� �������� �������� � ScrollView
        foreach (Transform child in scrollView.content)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddButtonClick(Dropdown dropdown)
    {
        // ������� ��������� ������ � ��� ������� ����
        GameObject newGameObject = objectManager.CreatePrimitiveObject(dropdown.value);
        CreateNewMenuElement(newGameObject);
    }

    private void CreateNewMenuElement(GameObject gameObject)
    {
        // ������� ������� ���� � �������� ���������
        GameObject newElement = Instantiate(MenuElementPrefab, scrollView.content);
        MenuElement elementComponent = newElement.GetComponent<MenuElement>();

        // ��������� � ��� ��������� � ����
        menuElements.Add(elementComponent);

        // ����������� � ������� � ����� ���
        elementComponent.LinkObject(gameObject);
        elementComponent.GetComponentInChildren<TMP_Text>().text = gameObject.name;
    }

    public void SetColorOnSelectedItems()
    {
        Color color = new Color(SliderR.value, SliderG.value, SliderB.value);

        foreach (MenuElement element in menuElements)
        {
            if (element.selectionToggle.isOn)
            {
                Renderer renderer = element.SceneObject.GetComponent<Renderer>();

                renderer.material.color = color;

            }
        }
    }

    public void SetColorOnImage()
    {
        Color color = new Color(SliderR.value, SliderG.value, SliderB.value);
        CheckColorImage.color = color;
    }

    public void SetCheckBoxAllItems(Toggle toggle)
    {
        foreach (MenuElement element in menuElements)
        {
            element.selectionToggle.isOn = toggle.isOn;
        }
    }

    public void SetVisibilitySelectedItems(Toggle toggle)
    {
        foreach (MenuElement element in menuElements)
        {
            if (element.selectionToggle.isOn)
            {
                element.visibilityToggle.isOn = toggle.isOn;
            }
        }
    }

    public void SetTransparencySelectedItems(Slider slider)
    {
        foreach (MenuElement element in menuElements)
        {
            if (element.selectionToggle.isOn)
            {
                Renderer gameObjectRenderer = element.SceneObject.GetComponent<Renderer>();
                Color color = gameObjectRenderer.material.color;
                color.a = slider.value;
                gameObjectRenderer.material.color = color;
            }
        }

    }

    public void SelectItem(MenuElement menuElement)
    {
        Color color = menuElement.SceneObject.GetComponent<Renderer>().material.color;

        // ���������� ���� ������� � ����
        SliderR.value = color.r;
        SliderG.value = color.g;
        SliderB.value = color.b;
        SliderA.value = color.a;
    }

    public void DeleteSelected()
    {
        List<MenuElement> deleteBuffer = new List<MenuElement>();

        // ������� ������� �� �����
        foreach (MenuElement element in menuElements)
        {
            if (element.selectionToggle.isOn)
            {
                objectManager.RemoveObject(element.SceneObject);
                deleteBuffer.Add(element);
            }
        }

        // ������� �������� � ����
        foreach (MenuElement element in deleteBuffer)
        {
            menuElements.Remove(element);
            Destroy(element.gameObject);
        }

    }
}
