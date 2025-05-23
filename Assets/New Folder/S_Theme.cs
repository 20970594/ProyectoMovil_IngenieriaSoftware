using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static AYellowpaper.SerializedCollections.SerializedDictionarySample;

namespace AYellowpaper.SerializedCollections
{
    public class S_Theme : MonoBehaviour
    {
        [SerializeField]
        public string themeName;
        [SerializeField]
        public string themeDescription;
        [SerializeField]
        [SerializedDictionary("LessonName", "Lesson")]
        public SerializedDictionary<string, S_Lesson> LessonList;
        //[SerializeField]
        //List<Minigame> MinigameList;
        //[SerializeField]
        //Challenge challenge;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
