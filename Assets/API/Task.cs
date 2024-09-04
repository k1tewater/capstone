[System.Serializable]
public class Task
{
    public int code;
    public Data data;

    public override string ToString()
    {
        return $"Code : {code}\nData : {data}";
    }
}

[System.Serializable]
public class Data
{
    public string task_id;
    public string type;
    public string status;
    public InputData input;
    public Output output;
    public int progress;
    public int create_time;
    public string prompt;
    public int running_left_time;
    public Result result;

    public override string ToString()
    {
        return $"task_id : {task_id}, type : {type}, status : {status}, \n" +
        $"input : {input}, output : {output}, progress : {progress}, \n" +
        $"create_time : {create_time}, prompt : {prompt}, running_left_time : {running_left_time}\n" +
        $"result : {result}";
    }
}

[System.Serializable]
public class Output
{
    public string model;

    public override string ToString()
    {
        return model;
    }
}

[System.Serializable]
public class InputData
{
    public string prompt;

    public override string ToString()
    {
        return prompt;
    }
}

[System.Serializable]
public class Result
{
    public GlbModel model;

    public override string ToString()
    {
        return model.ToString();
    }
}

[System.Serializable]
public class GlbModel
{
    public string type;
    public string url;

    public override string ToString()
    {
        return $"type : {type}, url : {url}";
    }
}