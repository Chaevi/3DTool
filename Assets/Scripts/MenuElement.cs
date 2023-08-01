using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuElement : MonoBehaviour
{
    private MenuInteractions mInteractions; // ��� ��������� � ����

    public Toggle visibilityToggle;
    public Toggle selectionToggle;

    [SerializeField] private TMP_Text elementText;

    public GameObject SceneObject { get; private set; } // ����������� ������

    public void LinkObject(GameObject sceneObject)
    {
        this.SceneObject = sceneObject;

        mInteractions = FindObjectOfType<MenuInteractions>();
    }

    public void SelectGameObject()
    {
        // ������ ������ ���� ��������
        FindObjectOfType<CameraController>().SetTarget(SceneObject.transform);

        // �������� ���� ������
        mInteractions.SelectItem(this);
    }

    public void HideObject()
    {
        SceneObject.GetComponent<Renderer>().enabled = visibilityToggle.isOn;
    }
}
