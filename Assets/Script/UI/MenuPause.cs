using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    private void OnEnable()
    {
        playerManager.ChangePlayerEnabledStatus();
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        playerManager.ChangePlayerEnabledStatus();
        Time.timeScale = 1;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Assets/Scenes/MainMenu.unity", LoadSceneMode.Single);
    }
}
