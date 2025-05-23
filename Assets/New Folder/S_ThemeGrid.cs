using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class S_ThemeGrid : MonoBehaviour
{
    public GameObject grid;
    public GameObject prefabThemeBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(S_Theme theme in S_AppManager.AppInstance.themeManager.ThemeList.Values)
        {
            GameObject temp = Instantiate(prefabThemeBtn);
            temp.transform.SetParent(grid.transform);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = theme.themeName;
            temp.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(S_SceneManager.instance.LoadLessonScene);
        }
    }

    public void A()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
