using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class S_LessonGrid : MonoBehaviour
{
    public GameObject grid;
    public GameObject prefabLessonBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (S_Lesson lesson in S_AppManager.AppInstance.currentTheme.LessonList.Values)
        {
            GameObject temp = Instantiate(prefabLessonBtn);
            temp.transform.SetParent(grid.transform);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = lesson.lessonName;
            temp.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => S_AppManager.AppInstance.UpdateCurrentLesson(lesson.lessonName));
            temp.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => S_SceneManager.instance.LoadScene("LessonScene"));
        }
    }
}
