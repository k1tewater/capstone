using UnityEngine;
using System.Text;
using System.Collections;
using UnityEngine.Networking;
using GLTFast;
public class APICaller
{
    private const string apiKey = "tsk_bYdKE9lIq14RjQQVrVO26lPn68GaHr8E4hEy2Tja67g";
    private const string url = "https://api.tripo3d.ai/v2/openapi/task";
    private const string uploadUrl = "https://api.tripo3d.ai/v2/openapi/upload";
    UnityWebRequest www;
    public Task task;
    byte[] modelBynary;

    public IEnumerator TextTo(string prompt)
    {
        yield return PostTextToTask(prompt);
        yield return SetTask();
        yield return SetModelBynary();
        InstantiateModel();
    }

    public IEnumerator ImageTo(Texture2D texture)
    {
        yield return PostImageUpload(texture);
        yield return PostImageToTask();
        yield return SetTask();
        yield return SetModelBynary();
        InstantiateModel();
    }



    IEnumerator PostImageUpload(Texture2D tex)
    {
        byte[] fileData = tex.EncodeToPNG();

        // MultipartFormDataSection을 사용하여 파일과 데이터 작성
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, "object", "image/png");

        // UnityWebRequest 생성, 헤더에 API 키 추가
        UnityWebRequest request = UnityWebRequest.Post(uploadUrl, form);
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // 요청을 보내고 응답을 대기
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Upload complete! Response: " + request.downloadHandler.text);
            task = JsonUtility.FromJson<Task>(request.downloadHandler.text);
        }
    }

    IEnumerator PostImageToTask()
    {
        string data = "{\"type\": \"image_to_model\", \"file\": { \"type\": \"png\", \"file_token\": \"" + task.data.image_token + "\"}}";
        byte[] dataraw = Encoding.UTF8.GetBytes(data);

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
    private IEnumerator PostTextToTask(string prompt)
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