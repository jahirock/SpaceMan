using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public void ShowGameMenu()
    {
        menuCanvas.enabled = true;
        gameCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }
    
    public void ShowGame()
    {
        menuCanvas.enabled = false;
        gameCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }

    public void ShowGameOver()
    {
        menuCanvas.enabled = false;
        gameCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    public void ExitGame()
    {
        //Si esta en el editor de unity se cierra de esta forma
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        //Si no esta en el editor de unity se pueden agregar otras formas, dependiendo de la plataforma.
        //https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
            Application.Quit();
        #endif

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
