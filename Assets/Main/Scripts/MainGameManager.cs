using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgMusic;

    private void Start()
    {
        //NewGame();
        bgMusic.Play();
    }
}
