using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() => SceneManager.LoadScene("MainScene");

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

    public void LoadCheckDeviceScene() => SceneManager.LoadScene("SampleScene");
    
}
