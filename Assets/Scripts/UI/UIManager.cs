using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text coinsText;
    public Text recordText;
    private Coins coins = Coins.SetInstance();
    public GameObject playerMenu;
    private bool isMenuOpened;
    public PlayerController player;
    public Text songName;
    public AudioManager audioManager;
    public void RestartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        isMenuOpened = false;
        playerMenu.SetActive(false); 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpened = !isMenuOpened;
        }
    }
    private void OnGUI()
    {
        coinsText.text = coins.getSetCoins.ToString();
        recordText.text = coins.setPoints.ToString("0.00");
        if (audioManager.audios.Count > 0)
        {
            songName.text = audioManager.audios[audioManager.currentSong].name;
        }

        if(isMenuOpened)
        {
            playerMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            
        }
        else if(player.isDead)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1;
        }
        else
        {
            playerMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        
    }
}
