using AYellowpaper.SerializedCollections;
using UnityEngine;

public class S_AppManager : MonoBehaviour
{
    public static S_AppManager AppInstance;
    public S_ThemeManager themeManager;
    private void Awake()
    {
        if (AppInstance == null)
        {
            AppInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
