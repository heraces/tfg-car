using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    private bool pause = false; 
    private bool outOfMenu = false;
    void Awake()
    {
        Screen.SetResolution(1024, 512, true);
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        outOfMenu = true;
    }

    public void BackToMenu()
    {
        pause = false;
        mainMenu.SetActive(true);
        outOfMenu = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        pause = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && outOfMenu) pause = !pause;

        if (pause)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
}
