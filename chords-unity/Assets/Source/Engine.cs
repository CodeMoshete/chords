using UnityEngine;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
    public Button AddNewButton;
    public GameObject ChordDiagramTemplate;
    public Transform ScrollContainer;

    private void Start()
    {
        AddNewButton.onClick.AddListener(AddNewChord);
    }

    private void AddNewChord()
    {
        GameObject newDiagram = Instantiate<GameObject>(ChordDiagramTemplate, ScrollContainer);
        newDiagram.transform.SetSiblingIndex(ScrollContainer.childCount - 2);
    }
}
