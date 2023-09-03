using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public static bool game_paused = false;
    public GameObject pauseMenu;
    

    
    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        game_paused = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(!game_paused) PauseLevel();

            else ResumeLevel();
        }
    }
    public void ResumeLevel()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        game_paused = false;
    }
    public void PauseLevel()
    {
        Time.timeScale=0;
        game_paused = true;
        pauseMenu.SetActive(true);
        

    }
    public void RestartLevel(){
        ResumeLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int sceneNumber)
    {     
        ResumeLevel();
        SceneManager.LoadScene(sceneNumber);
    }
}
