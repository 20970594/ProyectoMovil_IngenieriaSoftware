using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;

public class S_MemorizeGame : MonoBehaviour
{
    [Header("UI References")]
    public Image signImageDisplay;
    public List<Button> optionButtons;
    public TMP_Text feedbackText;
    public TMP_Text scoreText;
    public Button actionButton; // Botón dinámico Reinicio/Salir
    public TMP_Text actionButtonText;

    [Header("Game Settings")]
    public int roundsPerGame = 5;
    public float timeBetweenRounds = 1.5f;
    public Color defaultTextColor = Color.white;

    private S_Lesson currentLesson;
    private List<S_Sign> signsInLesson;
    private S_Sign currentSign;
    private int currentRound = 0;
    private int score = 0;
    private bool isWaitingForNextRound = false;
    private bool gameFinished = false;

    void Start()
    {
        currentLesson = S_AppManager.AppInstance.currentLesson;
        signsInLesson = currentLesson.SignList.Values.ToList();

        // Configurar botón de acción
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnActionButtonPressed);
        actionButtonText.text = "Reiniciar";
        actionButton.gameObject.SetActive(false);

        StartNewGame();
    }

    void StartNewGame()
    {
        currentRound = 0;
        score = 0;
        gameFinished = false;
        UpdateScore();
        actionButton.gameObject.SetActive(true);
        SetupNextRound();
    }

    void SetupNextRound()
    {
        if (currentRound >= roundsPerGame)
        {
            GameOver();
            return;
        }

        feedbackText.text = $"Ronda {currentRound + 1} de {roundsPerGame}";

        // Seleccionar una seña aleatoria
        currentSign = signsInLesson[Random.Range(0, signsInLesson.Count)];
        signImageDisplay.sprite = currentSign.SignImage;

        // Preparar opciones (1 correcta + 3 incorrectas)
        List<string> options = new List<string> { currentSign.signName };

        // Añadir opciones incorrectas (señas de la misma lección)
        List<S_Sign> incorrectSigns = signsInLesson.Where(s => s != currentSign).ToList();

        while (options.Count < 4 && incorrectSigns.Count > 0)
        {
            int randomIndex = Random.Range(0, incorrectSigns.Count);
            string wrongOption = incorrectSigns[randomIndex].signName;

            if (!options.Contains(wrongOption))
            {
                options.Add(wrongOption);
                incorrectSigns.RemoveAt(randomIndex);
            }
        }

        // Mezclar opciones
        options = options.OrderBy(x => Random.value).ToList();

        // Configurar botones
        for (int i = 0; i < optionButtons.Count; i++)
        {
            if (i < options.Count)
            {
                string option = options[i];
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = option;
                optionButtons[i].interactable = true;

                // Configurar listener
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(option));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnOptionSelected(string selectedOption)
    {
        // Desactivar todos los botones temporalmente
        foreach (var button in optionButtons)
        {
            button.interactable = false;
        }

        if (selectedOption == currentSign.signName)
        {
            // Respuesta correcta
            score++;
            feedbackText.text = "¡Correcto!";
            feedbackText.color = Color.green;
        }
        else
        {
            // Respuesta incorrecta
            feedbackText.text = $"Incorrecto. Era: {currentSign.signName}";
            feedbackText.color = Color.red;
        }

        // Actualizar el contador de rondas
        currentRound++;
        UpdateScore();

        // Preparar siguiente ronda después de un breve delay
        isWaitingForNextRound = true;
        Invoke("PrepareNextRound", timeBetweenRounds);
    }

    void PrepareNextRound()
    {
        isWaitingForNextRound = false;
        feedbackText.text = "";
        feedbackText.color = defaultTextColor;

        if (currentRound < roundsPerGame)
        {
            SetupNextRound();
        }
        else
        {
            GameOver();
        }
    }

    void UpdateScore()
    {
        scoreText.text = $"Puntuación: {score}/{currentRound}";
    }

    void GameOver()
    {
        gameFinished = true;
        feedbackText.text = $"Juego terminado! Puntuación final: {score}/{roundsPerGame}";

        // Cambiar el botón a "Salir"
        actionButtonText.text = "Salir";
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(ReturnToPreviousScene);
    }

    void OnActionButtonPressed()
    {
        if (gameFinished)
        {
            ReturnToPreviousScene();
        }
        else
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        // Cancelar cualquier invocación pendiente
        CancelInvoke();

        StartNewGame();
    }

    public void ReturnToPreviousScene()
    {
        S_SceneManager.instance.LoadPreviousScene();
    }
}