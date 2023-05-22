using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject covidGameSprite;
    public GameObject heartGameSpirte;
    public GameObject covidGameMain;
    public GameObject heartGameMain;

    private int sceneToLoad = 1; // default to covid
    [SerializeField] private int covidSceneID;
    [SerializeField] private int heartSceneID; // make sure to get the right sceneID after combining both games

    private void Update()
    {
        if(sceneToLoad == covidSceneID)
        {
            covidGameMain.SetActive(true);
            heartGameMain.SetActive(false);
        }
        else if(sceneToLoad == heartSceneID)
        {
            heartGameMain.SetActive(true);
            covidGameMain.SetActive(false);
        }
        else // set default to covid
        {
            covidGameMain.SetActive(true);
            heartGameMain.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneToLoad);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void SetSceneIDToSelectedGame(int choice)
    {
        if(choice == 0) // chose covid
        {
            covidGameSprite.SetActive(true);
            heartGameSpirte.SetActive(false);
            sceneToLoad = covidSceneID;
        }
        else if(choice == 1) // chose heart
        {
            heartGameSpirte.SetActive(true);
            covidGameSprite.SetActive(false);
            sceneToLoad = heartSceneID;
        }
        else // default set to covid
        {
            covidGameSprite.SetActive(true);
            heartGameSpirte.SetActive(false);
            sceneToLoad = covidSceneID;
        }
    }
}
