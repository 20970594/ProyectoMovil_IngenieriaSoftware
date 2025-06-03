using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class S_PopUpSignDetails : S_PopUpBase
{
    public S_Sign sign;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public Button playerButton;
    public TextMeshProUGUI textMeshPro;
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

    public override IEnumerator OpenPopUpSign(string name, S_Sign sign)
    {
        this.sign = sign;
        rawImage.texture = sign.renderTexture;
        videoPlayer.clip = sign.Video;
        videoPlayer.targetTexture = (RenderTexture)rawImage.texture;
        textMeshPro.text = sign.Description;

        yield return null;
    }
}
