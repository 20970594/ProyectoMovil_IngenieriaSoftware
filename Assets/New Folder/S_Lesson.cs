using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    public class S_Lesson : MonoBehaviour
    {
        [SerializeField]
        string Name;
        [SerializeField]
        string Description;
        [SerializedDictionary("SignName", "Sign")]
        public SerializedDictionary<string, S_Sign> SignList;
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
