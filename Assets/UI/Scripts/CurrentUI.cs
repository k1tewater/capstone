using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Current : MonoBehaviour
{
    public Stack<GameObject> ui = new Stack<GameObject>();

    void Start()
    {
        ui.Push(GameObject.Find("MainUI"));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject lastObject = ui.Peek();
            string currentName = lastObject.name;
            if(currentName == "MainUI")
            {
                Application.Quit();
            }
            else
            {
                ui.Pop().GetComponent<UIManager>().document.rootVisualElement.style.display = DisplayStyle.None;
            } 
        }
    }
}
