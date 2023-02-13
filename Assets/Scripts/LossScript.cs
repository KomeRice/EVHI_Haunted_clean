using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LossScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled && Input.anyKeyDown)
        {
            SceneManager.LoadScene("Main Menu");
            SetInputActive(true);
        }
    }
    public void SetInputActive(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = !newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
