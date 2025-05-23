using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    public class S_ThemeManager : MonoBehaviour
    {
        [SerializedDictionary("ThemeName", "Theme")]
        public SerializedDictionary<string, S_Theme> ThemeList;
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
