using UnityEngine;
using UnityEngine.Networking;

public class ShowHideGUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManagerHUD hud = this.gameObject.GetComponent<NetworkManagerHUD>();
        Debug.Log(hud);
        if (hud != null)
        {
            hud.offsetX = Screen.width / 2 - 75;
            hud.offsetY = Screen.height / 2 - 50;
        }
    }

    public void Show()
    {
        NetworkManagerHUD hud = this.gameObject.GetComponent<NetworkManagerHUD>();
        Debug.Log(hud);
        if (hud != null)
            hud.showGUI = true;
    }

    public void Hide()
    {
        NetworkManagerHUD hud = this.gameObject.GetComponent<NetworkManagerHUD>();
        if (hud != null)
            hud.showGUI = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
