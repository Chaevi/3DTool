using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuElement : MonoBehaviour
{
    private MenuInteractions mInteractions; // Для обращения к меню

    public Toggle visibilityToggle;
    public Toggle selectionToggle;

    [SerializeField] private TMP_Text elementText;

    public GameObject SceneObject { get; private set; } // Привязанный объект

    public void LinkObject(GameObject sceneObject)
    {
        this.SceneObject = sceneObject;

        mInteractions = FindObjectOfType<MenuInteractions>();
    }

    public void SelectGameObject()
    {
        // Задаем камере цель вращения
        FindObjectOfType<CameraController>().SetTarget(SceneObject.transform);

        // Передаем меню объект
        mInteractions.SelectItem(this);
    }

    public void HideObject()
    {
        SceneObject.GetComponent<Renderer>().enabled = visibilityToggle.isOn;
    }
}
