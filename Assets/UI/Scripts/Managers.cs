using System.Runtime.Serialization;

public static class Managers
{
    private static UIManager _uiInstance;
    public static UIManager ui
    {
        get
        {
            if (_uiInstance == null)
            {
                _uiInstance = new UIManager();
            }
            return _uiInstance;
        }
    }
}