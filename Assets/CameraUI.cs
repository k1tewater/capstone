using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class CameraUI : MonoBehaviour
{
    UIDocument document;
    List<Button> buttons;
    void Awake()
    {
        document = GetComponent<UIDocument>();
        buttons = new List<Button>();
        String[] buttonNames = {"Capture"};
        EventCallback<ClickEvent>[] clickEvts = {ClickCapture};
        
        for (int i = 0; i < buttonNames.Length; i++)
        {
            // 버튼을 가져와 리스트에 추가
            Button button = document.rootVisualElement.Q<Button>(buttonNames[i]);
            buttons.Add(button);

            // 각 버튼에 해당하는 메서드를 이벤트로 등록
            button.RegisterCallback<ClickEvent>(clickEvts[i]);
        }
    }

    void ClickCapture(ClickEvent evt)
    {
        Debug.Log("Imageto clicked");
    }

}
