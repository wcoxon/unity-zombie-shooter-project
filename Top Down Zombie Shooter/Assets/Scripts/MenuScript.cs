using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameScene");
    }
    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        Application.Quit();
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
