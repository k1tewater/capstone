using UnityEngine;
using System.Text;
using System.Collections;
using UnityEngine.Networking;
using GLTFast;
public class APICaller
{
    private const string apiKey = "tsk_bYdKE9lIq14RjQQVrVO26lPn68GaHr8E4hEy2Tja67g";
    private const string url = "https://api.tripo3d.ai/v2/openapi/task";
    UnityWebRequest www;
    public Task task;
    byte[] modelBynary;

    public IEnumerator TextTo(string prompt)
    {
        yield return PostTask(prompt);
        yield return SetTask();
        yield return SetModelBynary();
        InstantiateModel();
    }

    private IEnumerator PostTask(string prompt)
    {
        Debug.Log("Post Task start");
        string data = "{\"type\": \"text_to_model\", \"prompt\": \"" + prompt + "\"}";
        byte[] dataraw = Encoding.UTF8.GetBytes(data);

        if (prompt == null) { yield return null; }
        using (www = new UnityWebRequest(url, "POST"))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            www.uploadHandler = new UploadHandlerRaw(dataraw);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            task = JsonUtility.FromJson<Task>(www.downloadHandler.text);
            Debug.Log(task.ToString());
        }
    }

    private IEnumerator SetTask()
    {
        while (true)
        {
            if (task.data.task_id == null)
            {
                Debug.Log("task id is null");
                yield return null;
            }
            using (www = UnityWebRequest.Get(url + "/" + task.data.task_id))
            {
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                yield return www.SendWebRequest();
                task = JsonUtility.FromJson<Task>(www.downloadHandler.text);
            }

            if (task.data.running_left_time == -1)
            {
                Debug.Log("Task completed");
                break;
            }
        }
    }

    private IEnumerator SetModelBynary()
    {
        Debug.Log("Set model start");
        string modelURL = task.data.output.model;
        using (UnityWebRequest www = UnityWebRequest.Get(modelURL))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("modelBynary Setting");
                modelBynary = www.downloadHandler.data;
                Debug.Log($"modelBynary length : {modelBynary.Length}");
            }
        }
    }
    private async void InstantiateModel()
    {
        Debug.Log("InstantiateModel Start");
        var gltf = new GltfImport();
        await gltf.LoadGltfBinary(modelBynary);
        await gltf.InstantiateMainSceneAsync(GameObject.Find("Object").transform);
        Debug.Log("InstantiateModel Done");
    }
}