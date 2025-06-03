using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_SignGrid : MonoBehaviour
{
    public GameObject grid;
    public GameObject prefabSignBtn;
    public S_PopUpManager popUpManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (S_Sign sign in S_AppManager.AppInstance.currentLesson.SignList.Values)
        {
            GameObject signBtn = Instantiate(prefabSignBtn);
            signBtn.transform.SetParent(grid.transform);
            signBtn.GetComponentInChildren<TextMeshProUGUI>().text = sign.signName;
            signBtn.GetComponentsInChildren<Image>()[1].sprite = sign.SignImage;

            signBtn.GetComponent<S_Sign>().signName = sign.signName;
            signBtn.GetComponent<S_Sign>().Description = sign.Description;
            signBtn.GetComponent<S_Sign>().SignImage = sign.SignImage;
            signBtn.GetComponent<S_Sign>().Video = sign.Video;
            signBtn.GetComponent<S_Sign>().renderTexture = sign.renderTexture;

            signBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => popUpManager.OpenPopUpSign("SignDetails", sign));
        }
    }
}
