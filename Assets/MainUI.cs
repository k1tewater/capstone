using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class MainUI : MonoBehaviour
{
    UIDocument document;
    List<Button> buttons;
    void Awake()
    {
        document = GetComponent<UIDocument>();
        buttons = new List<Button>();
        String[] buttonNames = {"Imageto", "Textto", "List"};
        EventCallback<ClickEvent>[] clickEvts = {ClickImageTo, ClickTextTo, ClickList};
        
        for (int i = 0; i < buttonNames.Length; i++)
        {
            // 버튼을 가져와 리스트에 추가
            Button button = document.rootVisualElement.Q<Button>(buttonNames[i]);
            buttons.Add(button);

            // 각 버튼에 해당하는 메서드를 이벤트로 등록
            button.RegisterCallback<ClickEvent>(clickEvts[i]);
        }
    }

    void ClickImageTo(ClickEvent evt)
    {
        Debug.Log("Imageto clicked");
    }

    void ClickTextTo(ClickEvent evt)
    {
         Debug.Log("text to clicked");
    }

    void ClickList(ClickEvent evt)
    {
         Debug.Log("list clicked");
    }
}
