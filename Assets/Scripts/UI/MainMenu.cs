using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator cameraAnimator;
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
    public void Records()
    {
        mainMenu.SetActive(false);
        recordsWindow.SetActive(true);
    }

    public void ToShop()
    {
        StopAllCoroutines();
        cameraAnimator.Play("ToShop");
        mainMenu.SetActive(false);
        Debug.Log("It Work");
        StartCoroutine(OpenWindowAnimation(shopWindow, 1.25f));
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void FromShopToMenu()
    {
        Cancel(shopWindow, mainMenu, true, "ToMenuFromShop", 1.25f);
    }

    public void FromRecordToMenu()
    {
        Cancel(recordsWindow, mainMenu, false, "", 0);
    }
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

    private IEnumerator OpenWindowAnimation(GameObject window, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        window.SetActive(true);
    }
}
