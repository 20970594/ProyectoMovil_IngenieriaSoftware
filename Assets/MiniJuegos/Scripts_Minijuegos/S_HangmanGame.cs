using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;

public class S_HangmanGame : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text wordDisplay;
    public Image hangmanImage;
    public List<Button> optionButtons;
    public List<Image> optionSignImages;
    public TMP_Text feedbackText;
    public Button actionButton;
    public TMP_Text actionButtonText;

    [Header("Game Settings")]
    public Sprite[] hangmanStages;
    public string[] wordsForLesson;
    public int roundsPerGame = 5;
    public float timeBetweenRounds = 1.5f;
    public Color defaultTextColor = Color.white;

    private string currentWord;
    private char[] revealedLetters;
    private List<char> availableLetters;
    private int livesRemaining;
    private S_Lesson currentLesson;
    private int currentRound = 0;
    private int score = 0;
    private bool gameFinished = false;

    void Start()
    {
        currentLesson = S_AppManager.AppInstance.currentLesson;

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
        actionButton.gameObject.SetActive(true);
        UpdateFeedback();
        InitializeRound();
    }

    void InitializeRound()
    {
        if (currentRound >= roundsPerGame)
        {
            GameOver();
            return;
        }

        currentRound++;
        UpdateFeedback();

        // Seleccionar una palabra aleatoria para la lección actual
        currentWord = GetRandomWordForLesson();
        availableLetters = GetLettersFromCurrentLesson();
        revealedLetters = new char[currentWord.Length];

        // Inicializar palabra mostrada
        for (int i = 0; i < currentWord.Length; i++)
        {
            revealedLetters[i] = availableLetters.Contains(char.ToUpper(currentWord[i])) ? '_' : char.ToUpper(currentWord[i]);
        }

        UpdateWordDisplay();
        livesRemaining = hangmanStages.Length - 1;
        hangmanImage.sprite = hangmanStages[0];

        SetupOptions();
    }

    void UpdateFeedback()
    {
        if (gameFinished)
        {
            feedbackText.text = $"Juego terminado!\nAciertos: {score}/{roundsPerGame}";
        }
        else
        {
            feedbackText.text = $"Ronda {currentRound} de {roundsPerGame}\nAciertos: {score}";
        }
        feedbackText.color = defaultTextColor;
    }

    string GetRandomWordForLesson()
    {
        return wordsForLesson[Random.Range(0, wordsForLesson.Length)].ToUpper();
    }

    List<char> GetLettersFromCurrentLesson()
    {
        List<char> letters = new List<char>();
        foreach (var sign in currentLesson.SignList.Values)
        {
            if (sign.signName.Length == 1)
            {
                letters.Add(char.ToUpper(sign.signName[0]));
            }
        }
        return letters;
    }

    void SetupOptions()
    {
        char correctLetter = GetMissingLetter();
        List<char> options = new List<char> { correctLetter };

        // Añadir letras incorrectas pero de la lección actual
        var lessonLetters = GetLettersFromCurrentLesson();
        lessonLetters.Remove(correctLetter);

        while (options.Count < 4 && lessonLetters.Count > 0)
        {
            int randomIndex = Random.Range(0, lessonLetters.Count);
            if (!options.Contains(lessonLetters[randomIndex]))
            {
                options.Add(lessonLetters[randomIndex]);
                lessonLetters.RemoveAt(randomIndex);
            }
        }

        // Mezclar opciones
        options = options.OrderBy(x => Random.value).ToList();

        // Configurar botones
        for (int i = 0; i < optionButtons.Count; i++)
        {
            if (i < options.Count)
            {
                char option = options[i];
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = option.ToString();
                optionButtons[i].interactable = true;

                // Buscar la seña correspondiente
                S_Sign sign = currentLesson.SignList.Values.FirstOrDefault(s => s.signName.ToUpper() == option.ToString());
                if (sign != null)
                {
                    optionSignImages[i].sprite = sign.SignImage;
                    optionButtons[i].onClick.RemoveAllListeners();

                    char selectedOption = option;
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(selectedOption));
                }
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    char GetMissingLetter()
    {
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (revealedLetters[i] == '_' && availableLetters.Contains(char.ToUpper(currentWord[i])))
            {
                return char.ToUpper(currentWord[i]);
            }
        }
        return '_';
    }

    void OnOptionSelected(char selectedLetter)
    {
        // Desactivar botones temporalmente
        foreach (var button in optionButtons)
        {
            button.interactable = false;
        }

        bool correctGuess = false;

        // Revelar letras correctas
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (char.ToUpper(currentWord[i]) == selectedLetter)
            {
                revealedLetters[i] = selectedLetter;
                correctGuess = true;
            }
        }

        if (correctGuess)
        {
            feedbackText.text = "¡Correcto!";
            feedbackText.color = Color.green;
            UpdateWordDisplay();

            // Verificar si completó la palabra
            if (!revealedLetters.Contains('_'))
            {
                score++;
                UpdateFeedback();
                feedbackText.text += "\n¡Palabra completada!";
                Invoke("PrepareNextRound", timeBetweenRounds);
            }
            else
            {
                // Continuar con la misma palabra
                SetupOptions();
            }
        }
        else
        {
            livesRemaining--;
            hangmanImage.sprite = hangmanStages[hangmanStages.Length - livesRemaining - 1];
            feedbackText.text = "Incorrecto";
            feedbackText.color = Color.red;

            if (livesRemaining <= 0)
            {
                feedbackText.text += $"\nLa palabra era: {currentWord}";
                Invoke("PrepareNextRound", timeBetweenRounds);
            }
            else
            {
                SetupOptions();
            }
        }
    }

    void PrepareNextRound()
    {
        if (currentRound < roundsPerGame)
        {
            InitializeRound();
        }
        else
        {
            GameOver();
        }
    }

    void UpdateWordDisplay()
    {
        wordDisplay.text = string.Join(" ", revealedLetters);
    }

    void GameOver()
    {
        gameFinished = true;
        UpdateFeedback();

        // Cambiar botón a "Salir"
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
        CancelInvoke();
        StartNewGame();
    }

    public void ReturnToPreviousScene()
    {
        S_SceneManager.instance.LoadPreviousScene();
    }
}