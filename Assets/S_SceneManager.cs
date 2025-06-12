using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class S_SceneManager : MonoBehaviour
{
    public static S_SceneManager instance;
    [SerializeField]
    private string currentSceneName;
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private GameObject fadeScenePrefab;
    private GameObject fadeScene;
    private Material fadeMaterial;
    public float duration = 0.5f;
    Coroutine fadeRoutine;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        currentSceneName = SceneManager.GetActiveScene().name;
        fadeScene = Instantiate(fadeScenePrefab);
        fadeMaterial = fadeScene.GetComponentInChildren<Image>().material;
        fadeMaterial.SetFloat("_Fade", 1);
        FadeOut();
        if(GameObject.FindGameObjectWithTag("ExitBtn") != null)
        {
            GameObject.FindGameObjectWithTag("ExitBtn").GetComponent<Button>().onClick.AddListener(LoadPreviousScene);
        }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*currentSceneName = SceneManager.GetActiveScene().name;
        fadeScene = Instantiate(fadeScene);
        fadeMaterial = fadeScene.GetComponentInChildren<Image>().material;
        fadeMaterial.SetFloat("_Fade", 1);
        FadeOut();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Metodos de presionar botones de temas
    public void ThemeButtonPressed()
    {
        GameObject buttonPressed = EventSystem.current.currentSelectedGameObject;

        if (buttonPressed != null)
        {
            var button = buttonPressed.GetComponent<Button>();
            if (button != null) {
                Debug.Log("Se presiono " + buttonPressed.name);
            }
        }
    }

    //Metodos de carga de niveles-----------------------------------------------------
    public void LoadPreviousScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch(currentScene) {
            case "LessonScene":
                SceneManager.LoadScene("Lessons");
                break;
            case "Lessons":
                SceneManager.LoadScene("MainmenuScene");
                break;
        }
        
    }
    public void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        FadeIn();
    }
    public void LoadMainmenuScene()
    {
        nextSceneName = "MainmenuScene";
        FadeIn();
    }
    /*public void LoadLessonScene()
    {
        nextSceneName = "Lessons";
        FadeIn();
    }*/

    //Metodos de FadeIn y FadeOut-----------------------------------------------------
    public void FadeIn()
    {
        StartFade(1f);
    }
    public void FadeOut()
    {
        StartFade(0f);
    }

    void StartFade(float targetAlpha)
    {
        if (fadeRoutine != null) { 
            StopCoroutine(fadeRoutine);
        }
        fadeScene.GetComponent<CanvasGroup>().blocksRaycasts = true;
        fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha));
    }
    void OnFadeComplete(float In_Out)
    {
        fadeScene.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (In_Out <= 0f)
        {
        }
        else if(In_Out >= 1f)
        {
            switch (nextSceneName)
            {
                case "MainmenuScene":
                    SceneManager.LoadScene(nextSceneName);
                    break;
                case "Lessons":
                    SceneManager.LoadScene(nextSceneName);
                    break;
                case "LessonScene":
                    SceneManager.LoadScene(nextSceneName);
                    break;
            }
        }
    }
    public IEnumerator FadeRoutine(float target)
    {
        float start = fadeMaterial.GetFloat("_Fade");
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float newFade = Mathf.Lerp(start, target, t);
            fadeMaterial.SetFloat("_Fade", newFade);
            yield return null;
        }

        fadeMaterial.SetFloat("_Fade", target); // asegurarse del valor final
        OnFadeComplete(target);
    }
}
