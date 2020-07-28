using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    /// <summary>
    /// Текст с количеством монеток
    /// </summary>
    public Text coinsText;

    /// <summary>
    /// Текст с рекордом
    /// </summary>
    public Text recordText;

    /// <summary>
    /// Инстанс монеток
    /// </summary>
    private Coins coins = Coins.SetInstance();

    /// <summary>
    /// Объект с меню игрока
    /// </summary>
    public GameObject playerMenu;

    /// <summary>
    /// Проверка, открыто ли меню
    /// </summary>
    public bool isMenuOpened;

    /// <summary>
    /// Контроллер игрока
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// Текст с названием музыки
    /// </summary>
    public Text songName;

    /// <summary>
    /// Контроллер аудио
    /// </summary>
    public AudioManager audioManager;

    /// <summary>
    /// Перезапуск сцены
    /// </summary>
    public void RestartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Возвращает игрока в игру
    /// </summary>
    public void Continue()
    {
        isMenuOpened = false;
        playerMenu.SetActive(false); 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Выходит в главное меню
    /// </summary>
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
        //Обновляем элементы интерфейса
        coinsText.text = coins.getSetCoins.ToString();
        recordText.text = coins.setPoints.ToString("0.00");
        if (audioManager.audios.Count > 0)
        {
            songName.text = audioManager.audios[audioManager.currentSong].name;
        }

        //Регулируем курсор
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
