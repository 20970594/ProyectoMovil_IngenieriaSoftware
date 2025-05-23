using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

namespace AYellowpaper.SerializedCollections { 
    public class S_PopUpManager : MonoBehaviour
    {
        [SerializedDictionary("PopUpName", "PopUpPrefab")]
        public SerializedDictionary<string, GameObject> PopUpList;

        public SerializedDictionary<string, GameObject> ActivePopUps;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenPopUp(string name)
        {
            AddActivePopUp(name, PopUpList[name]);
            Debug.Log("Opening: " + name + "/ "+ ActivePopUps[name].GetComponent<S_PopUpBase>().popUpName);

            ActivePopUps[name].GetComponent<S_PopUpBase>().StartOpenPopUp();
        }

        public void AddActivePopUp(string name, GameObject popUp)
        {
            if (ActivePopUps.ContainsKey(name))
            {
                ActivePopUps.Remove(name);
                ActivePopUps.Add(name, Instantiate(PopUpList[name]));
            }
            else
            {
                ActivePopUps.Add(name, Instantiate(PopUpList[name]));
            }
        }
    }
}
