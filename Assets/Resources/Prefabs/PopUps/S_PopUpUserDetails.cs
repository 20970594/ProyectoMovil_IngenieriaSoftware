using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class S_PopUpUserDetails : S_PopUpBase
{
    public override IEnumerator ClosePopUp()
    {
        Debug.Log("Hola soy UserDetails");
        Destroy(this.gameObject);
        yield return null;
    }

    public override IEnumerator AcceptPopUp()
    {
        Debug.Log("Hacer comportamiento UserDetails");
        yield return null;
    }
    public override IEnumerator OpenPopUp()
    {
        Debug.Log("Abriendo hijo");
        yield return null;
    }
}
