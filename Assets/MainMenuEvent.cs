using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvent : MonoBehaviour
{
    private UIDocument document;
    public Button imagebutton;
    public Button camerabutton;
    GameObject imageUI;
    GameObject cameraUI;
    ProgressBar progressBar;
   
    private void Awake()
    {
        
        imageUI = GameObject.Find("image");
        cameraUI = GameObject.Find("Camera");
        


        imagebutton = GetComponent<UIDocument>().rootVisualElement.Q("image") as Button;
        camerabutton = imageUI.GetComponent<UIDocument>().rootVisualElement.Q("Camera") as Button;
        progressBar = cameraUI.GetComponent<UIDocument>().rootVisualElement.Q("ProgressBar1") as ProgressBar;

        imagebutton.RegisterCallback<ClickEvent>(imagetoClick);
        camerabutton.RegisterCallback<ClickEvent>(CameraClick);

        

        // ProgressBar의 범위 설정
        progressBar.lowValue = 0;
        progressBar.highValue = 1;
        progressBar.title = "집가고싶다씨발";
        // ProgressBar의 value 설정
        progressBar.value = 0;

        //imageUI.SetActive(false);
        cameraUI.SetActive(false);
        
    }
    
    void Update()
    {
        progressBar.value += 0.01f;
    }

    private void imagetoClick(ClickEvent evt)
    {
        Debug.Log("image button clicked");
        gameObject.SetActive(false);
        imageUI.SetActive(true);
    }

    private void CameraClick(ClickEvent evt)
    {
        Debug.Log("camera button clicked");
        imageUI.SetActive(false);
        cameraUI.SetActive(true);
    }
}
