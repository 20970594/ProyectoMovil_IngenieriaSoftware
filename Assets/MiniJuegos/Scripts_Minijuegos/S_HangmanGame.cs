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
    public TMP_Text descriptionText;

    [Header("Game Settings")]
    public Sprite[] hangmanStages; // Imágenes para cada etapa del ahorcado
    public string[] wordsForLesson; // Palabras para la lección actual

    private string currentWord;
    private char[] revealedLetters;
    private List<char> availableLetters;
    private int livesRemaining;
    private S_Lesson currentLesson;

    void Start()
    {
        currentLesson = S_AppManager.AppInstance.currentLesson;
        InitializeGame();
    }

    void InitializeGame()
    {
        // Seleccionar una palabra aleatoria para la lección actual
        currentWord = GetRandomWordForLesson();
        revealedLetters = new char[currentWord.Length];
        availableLetters = GetLettersFromCurrentLesson();

        // Inicializar palabra mostrada
        for (int i = 0; i < currentWord.Length; i++)
        {
            // Mostrar solo letras que NO están en la lección actual
            revealedLetters[i] = availableLetters.Contains(char.ToUpper(currentWord[i])) ? '_' : char.ToUpper(currentWord[i]);
        }

        UpdateWordDisplay();
        livesRemaining = hangmanStages.Length - 1;
        hangmanImage.sprite = hangmanStages[0];

        // Configurar primera ronda de opciones
        SetupOptions();
    }

    string GetRandomWordForLesson()
    {
        // Lógica para obtener palabras relevantes para la lección
        // Puedes expandir esto para tener un diccionario por lección
        return wordsForLesson[Random.Range(0, wordsForLesson.Length)].ToUpper();
    }

    List<char> GetLettersFromCurrentLesson()
    {
        List<char> letters = new List<char>();
        foreach (var sign in currentLesson.SignList.Values)
        {
            if (sign.signName.Length == 1) // Asumiendo que las letras son signos con nombre de 1 carácter
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

                // Buscar la seña correspondiente en la lección actual
                S_Sign sign = currentLesson.SignList.Values.FirstOrDefault(s => s.signName.ToUpper() == option.ToString());
                if (sign != null)
                {
                    optionSignImages[i].sprite = sign.SignImage;
                    optionButtons[i].onClick.RemoveAllListeners();

                    // Usar variable local para evitar problemas de closure
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
        return '_'; // Fallback, no debería ocurrir
    }

    void OnOptionSelected(char selectedLetter)
    {
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
            UpdateWordDisplay();

            // Verificar si ganó
            if (!revealedLetters.Contains('_'))
            {
                GameOver(true);
                return;
            }

            // Configurar nuevas opciones
            SetupOptions();
        }
        else
        {
            livesRemaining--;
            hangmanImage.sprite = hangmanStages[hangmanStages.Length - livesRemaining - 1];

            if (livesRemaining <= 0)
            {
                GameOver(false);
            }
            else
            {
                SetupOptions();
            }
        }
    }

    void UpdateWordDisplay()
    {
        wordDisplay.text = string.Join(" ", revealedLetters);
    }

    void GameOver(bool won)
    {
        foreach (var button in optionButtons)
        {
            button.interactable = false;
        }

        descriptionText.text = won ? "¡Felicidades! Ganaste." : $"Game Over. La palabra era: {currentWord}";

        // Mostrar botón de reinicio o continuar
        // Puedes añadir lógica para guardar progreso, puntos, etc.
    }

    public void RestartGame()
    {
        InitializeGame();
        foreach (var button in optionButtons)
        {
            button.interactable = true;
        }
        descriptionText.text = "";
    }
}