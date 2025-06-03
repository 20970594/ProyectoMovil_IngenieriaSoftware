using System.Collections;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class S_PopUpBase : MonoBehaviour
{
    public string popUpName;
    public Button ExitButton;
    public S_PopUpManager manager;
    public S_SceneManager sceneManager;

    public void Start()
    {
        if(sceneManager == null)
        {
            sceneManager = FindAnyObjectByType<S_SceneManager>();
        }
        if (ExitButton != null)
        {
            Debug.Log("closeAdded");
            ExitButton.onClick.AddListener(StartClosePopUp);
        }
        else
        {
            Debug.Log("closeNotAdded");
        }
    }

    public void StartOpenPopUp()
    {
        StartCoroutine(OpenPopUp());
    }
    public void StartOpenPopUpSign(string name, S_Sign sign)//esto requeriere refactorización
    {
        StartCoroutine(OpenPopUpSign(name, sign));
    }

    public void StartClosePopUp()
    {
        StartCoroutine (ClosePopUp());
    }

    public void StartAcceptPopUp()
    {
        StartCoroutine(AcceptPopUp());
    }

    public virtual IEnumerator ClosePopUp()
    {
        Destroy(this.gameObject);
        yield return null;
    }
    public virtual IEnumerator AcceptPopUp()
    {
        Debug.Log("Hacer comportamiento padre");
        yield return null;
    }
    public virtual IEnumerator OpenPopUp()
    {
        Debug.Log("Abriendo padre");
        yield return null;
    }
    public virtual IEnumerator OpenPopUpSign(string name, S_Sign sign)//esto requeriere refactorización
    {
        Debug.Log("Abriendo padre");
        yield return null;
    }
}
