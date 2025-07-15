using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneManagerHelper : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        S_SceneManager.instance.LoadScene(sceneName);
    }
    public void LoadPreviousScene(string sceneName)
    {
        S_SceneManager.instance.LoadPreviousScene();
    }
}
