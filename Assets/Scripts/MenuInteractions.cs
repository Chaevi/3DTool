using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteractions : MonoBehaviour
{
    ObjectManager objectManager; // Для получения отфильтрованных объектов

    public GameObject MenuElementPrefab; // Для создания элемента меню объекта сцены
    public ScrollRect scrollView; // Получение позиции создания

    private List<MenuElement> menuElements = new List<MenuElement>(); // Пул меню элементов

    // Слайдеры изменения цвета
    [SerializeField] private Slider SliderR;
    [SerializeField] private Slider SliderG;
    [SerializeField] private Slider SliderB;
    [SerializeField] private Slider SliderA;

    // Картинка для предпросмотра цвета
    [SerializeField] private Image CheckColorImage;

    private bool isShowed = false; // Отображение меню

    private void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
        GenerateScrollView();
    }

    public void ToggleMenu()
    {
        // Включние/выключение меню
        if (isShowed)
            isShowed = false;
        else
            isShowed = true;

        this.gameObject.SetActive(isShowed);
    }

    private void GenerateScrollView()
    {
        List<GameObject> sceneObjects = objectManager.GetListObjects();
        // Очищаем Scroll View перед созданием новых элементов

        // Проверка на пустоту
        if (sceneObjects.Count < 1)
            return;

        ClearScrollView();

        // Создаем UI элементы для каждого объекта из пула
        foreach (GameObject obj in sceneObjects)
        {
            // Создаем экземпляр префаба UI элемента
            CreateNewMenuElement(obj);
        }
    }

    private void ClearScrollView()
    {
        // Удаляем все дочерние элементы в ScrollView
        foreach (Transform child in scrollView.content)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddButtonClick(Dropdown dropdown)
    {
        // Создаем выбранный объект и его элемент меню
        GameObject newGameObject = objectManager.CreatePrimitiveObject(dropdown.value);
        CreateNewMenuElement(newGameObject);
    }

    private void CreateNewMenuElement(GameObject gameObject)
    {
        // Создаем элемент меню и получаем компонент
        GameObject newElement = Instantiate(MenuElementPrefab, scrollView.content);
        MenuElement elementComponent = newElement.GetComponent<MenuElement>();

        // Добавляем в пул элементов в меню
        menuElements.Add(elementComponent);

        // Привязываем к объекту и пишем имя
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

        // Отображаем цвет объекта в меню
        SliderR.value = color.r;
        SliderG.value = color.g;
        SliderB.value = color.b;
        SliderA.value = color.a;
    }

    public void DeleteSelected()
    {
        List<MenuElement> deleteBuffer = new List<MenuElement>();

        // Удаляем объекты на сцене
        foreach (MenuElement element in menuElements)
        {
            if (element.selectionToggle.isOn)
            {
                objectManager.RemoveObject(element.SceneObject);
                deleteBuffer.Add(element);
            }
        }

        // Удаляем элементы в меню
        foreach (MenuElement element in deleteBuffer)
        {
            menuElements.Remove(element);
            Destroy(element.gameObject);
        }

    }
}
