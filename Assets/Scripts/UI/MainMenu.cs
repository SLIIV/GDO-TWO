using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    /// <summary>
    /// Аниматор камеры
    /// </summary>
    public Animator cameraAnimator;

    /// <summary>
    /// Объект с главным меню
    /// </summary>
    public GameObject mainMenu;
    public Text coins;

    /// <summary>
    /// Окно с рекордами
    /// </summary>
    [Header("Рекорды")]
    public GameObject recordsWindow;
    public Text playerRecord;

    /// <summary>
    /// Окно с магазином
    /// </summary>
    [Header("Магазин")]
    public GameObject shopWindow;


    public void Start()
    {
        //Получаем статистику игрока
        coins.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        playerRecord.text = PlayerPrefs.GetFloat("Points", 0).ToString("0.00");
        Time.timeScale = 1;
    }

    /// <summary>
    /// Начать игру
    /// </summary>
    public void StartTheGame()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Перейти в окно рекордов
    /// </summary>
    public void Records()
    {
        mainMenu.SetActive(false);
        recordsWindow.SetActive(true);
    }

    /// <summary>
    /// Переход в магазин
    /// </summary>
    public void ToShop()
    {
        StopAllCoroutines();
        cameraAnimator.Play("ToShop");
        mainMenu.SetActive(false);
        StartCoroutine(OpenWindowAnimation(shopWindow, 1.25f));
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Переход из магазина в меню
    /// </summary>
    public void FromShopToMenu()
    {
        Cancel(shopWindow, mainMenu, true, "ToMenuFromShop", 1.25f);
    }

    /// <summary>
    /// Переход с рекордов в меню
    /// </summary>
    public void FromRecordToMenu()
    {
        Cancel(recordsWindow, mainMenu, false, "", 0);
    }

    /// <summary>
    /// Отмена перехода
    /// </summary>
    /// <param name="objectToClose">Объект, который необходимо закрыть</param>
    /// <param name="objectToOpen">Объект, который необходимо открыть</param>
    /// <param name="isAnimeted">Анимированный ли переход</param>
    /// <param name="animName">Имя анимации (если есть)</param>
    /// <param name="delay">задержка между закрытием и появлением окна</param>
    public void Cancel(GameObject objectToClose, GameObject objectToOpen, bool isAnimeted, string animName, float delay)
    {
        if(isAnimeted)
        {
            StopAllCoroutines();
            cameraAnimator.Play(animName);
            objectToClose.SetActive(false);
            StartCoroutine(OpenWindowAnimation(objectToOpen, delay));
        }
        else
        {
            objectToClose.SetActive(false);
            objectToOpen.SetActive(true);
        }
    }

    /// <summary>
    /// Открытие окна с задержкой
    /// </summary>
    /// <param name="window">Объект с окном</param>
    /// <param name="delay">Задержка</param>
    /// <returns></returns>
    private IEnumerator OpenWindowAnimation(GameObject window, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        window.SetActive(true);
    }
}
