using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /**
     * Load the main scene
     */
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    /**
     * Exit the application (or play mode when in dev mode)
     */
    public void Exit()
    {
        //MainManager.Instance.SaveColor();

        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            // production/runtime code to quit Unity player
            Application.Quit(); 
        #endif
    }
}
