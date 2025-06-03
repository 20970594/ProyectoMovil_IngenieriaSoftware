using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;

public class S_AppManager : MonoBehaviour
{
    public static S_AppManager AppInstance;
    public S_ThemeManager themeManager;
    public S_Theme currentTheme;
    public S_Lesson currentLesson;
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

    public void UpdateCurrentTheme(string themeName)
    {
        Debug.Log("changing current theme to" + themeName);
        currentTheme = themeManager.ThemeList[themeName];
    }

    public void UpdateCurrentLesson(string lessonName)
    {
        currentLesson = currentTheme.LessonList[lessonName];
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
