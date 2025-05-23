using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class S_PopUpLogin : S_PopUpBase
{

    public override IEnumerator ClosePopUp()
    {
        Debug.Log("Hola soy login");
        Destroy(this.gameObject);
        yield return null;
    }

    public override IEnumerator AcceptPopUp()
    {
        Debug.Log("Hacer comportamiento register");
        sceneManager.LoadScene("MainmenuScene");
        yield return null;
    }
    public override IEnumerator OpenPopUp()
    {
        Debug.Log("Abriendo hijo");
        yield return null;
    }
}
