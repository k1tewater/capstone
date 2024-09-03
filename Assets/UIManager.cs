using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class UIManager : MonoBehaviour
{
    GameObject[] uiObjects;
    void Awake()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        uiObjects = allObjects.Where(obj => obj.name.EndsWith("UI")).ToArray();

        foreach(GameObject ui in uiObjects)
        {
            ui.GetComponent<UIDocument>();
        }
    }
}
