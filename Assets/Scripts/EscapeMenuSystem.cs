using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuSystem : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject player;

    private bool _menuActive = false;

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        player.GetComponent<PlayerControlUtils>().ToggleMenu();
        menuCanvas.SetActive(!_menuActive);
        _menuActive = !_menuActive;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
