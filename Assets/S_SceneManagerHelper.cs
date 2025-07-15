using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneManagerHelper : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        S_SceneManager.instance.LoadScene(sceneName);
    }

    public void LoadPreviousScene()
    {
        S_SceneManager.instance.LoadPreviousScene();
    }

    public void LoadRandomMinigame()
    {
        // Lista de escenas de minijuegos disponibles
        string[] minigameScenes = {
            "Minigame_HangmanScene",
            "Minigame_MemorizeScene"
        };

        // Seleccionar una escena al azar
        int randomIndex = Random.Range(0, minigameScenes.Length);
        string selectedScene = minigameScenes[randomIndex];

        // Cargar la escena seleccionada
        S_SceneManager.instance.LoadScene(selectedScene);
    }
}