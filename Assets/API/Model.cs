using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Model : MonoBehaviour
{
    APICaller apiCaller;
    TextMeshProUGUI tmp;
    bool isNeedUpdate = true;
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        apiCaller = new APICaller();
        StartCoroutine(apiCaller.PostTask("gray sprite cat"));
    }

    void Update()
    {
            if (isNeedUpdate && Time.frameCount % 60 == 0 && apiCaller.task != null)
            {
                if(apiCaller.task.data.running_left_time == -1)
                {
                isNeedUpdate = false;
                StartCoroutine(apiCaller.SetModel());
                }
                StartCoroutine(apiCaller.SetTask());
                tmp.text = apiCaller.task.data.progress + "%";    
            }
        
    }

    public void ClickButton()
    {
        Debug.Log("Instaintate Model start");
        apiCaller.InstantiateModelPlz();
    }

    public void ClickPrintButton()
    {
        Debug.Log("Print TaskData");
        Debug.Log(apiCaller.task);
    }
}
