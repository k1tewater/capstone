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



    public IEnumerator PostTask(string prompt)
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

    public IEnumerator SetTask()
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
    }

    public IEnumerator SetModel()
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

    public IEnumerator InstantiateModel(string objectName)
    {
        yield return SetModel();

        if (modelBynary != null)
        {
            var gltf = new GltfImport();
            Debug.Log("start load gltf binary");
            yield return gltf.LoadGltfBinary(modelBynary);
            GameObject modelObj = GameObject.Find(objectName);
            Debug.Log("start instantiate model");
            yield return gltf.InstantiateMainSceneAsync(modelObj.transform);
        }
    }
    public async void InstantiateModelPlz()
    {
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(modelBynary);
        if (success)
        {
            success = await gltf.InstantiateMainSceneAsync(GameObject.Find("Model").transform);
        }

    }
}