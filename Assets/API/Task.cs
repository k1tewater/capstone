[System.Serializable]
public class Task
{
    public int code;
    public Data data;

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
    public string image_token;
}

[System.Serializable]
public class Output
{
    public string model;
}

[System.Serializable]
public class InputData
{
    public string prompt;
}

[System.Serializable]
public class Result
{
    public GlbModel model;
}

[System.Serializable]
public class GlbModel
{
    public string type;
    public string url;
}