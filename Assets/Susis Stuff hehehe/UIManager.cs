using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectMask2D heatGauge;
    public Image Fam;

    public bool paused;
    public GameObject pauseMenu;
    public Sprite fireFine, fireDire, fireAwesome;
    public Image face;

    public static UIManager instance;

    [SerializeField] private GameObject StatusMenu;
    [SerializeField] private GameObject StatusText;
    private string Statustext;
    [SerializeField] private string tooHot;
    [SerializeField] private string tooCold;
    [SerializeField] private string winText;
    [SerializeField] private string death;
    private string statusText;

    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject tutorialText;

    [SerializeField] private GameObject raidAlert;
    [SerializeField] private GameObject hotSpotAlert;

    [SerializeField] private GameObject fadePanel;

    bool statusScreen;
    bool win;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float modHeat = HeatGauge.heat;
        heatGauge.padding = new Vector4(0, 0, (float)Mathf.Abs(modHeat * 9f - 900), 0);

        Fam.rectTransform.localPosition = new Vector3(0f, Mathf.Clamp(500 - (HeatGauge.time * 3), -400, 500), 0f);
        if (Input.GetKeyDown(KeyCode.R) && statusScreen)
        {
            if (win)
            {
                Time.timeScale = 1f;
                win = false;
                SceneSwap("LevelOne");
            } else
            {
                Time.timeScale = 1f;
                SceneSwap("Current Level");
            }
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        Pause();
    }
    public void Pause()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            paused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = paused;
        pauseMenu.SetActive(paused);
    }
    public void SceneSwap(string newLevel)
    {
        if (newLevel == "Current Level")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(newLevel);
        }
    }

    public void UpdateFace(int hp)
    {
        if(hp <= 2)
        {
            face.sprite = fireDire;
        } 
        else if (hp > 2 && hp <= 4)
        {
            face.sprite = fireFine;
        }
        else
        {
            face.sprite = fireAwesome;
        }
    }

    public void Status(string method)
    {
        switch (method)
        {
            case "tooHot":
                statusText = tooHot;
                break;
            case "tooCold":
                statusText = tooCold;
                break;
            case "win":
                statusText = winText;
                win = true;
                break;
            default:
                statusText = death;
                break;
        }
        Cursor.visible = true;
        Cursor.lockState =  CursorLockMode.None;
        Time.timeScale = 0f;
        StatusMenu.SetActive(true);
        StatusText.GetComponent<TextMeshProUGUI>().text = statusText;
        statusScreen = true;
    }

    public void Tutorialize(string text)
    {
        Time.timeScale = 0f;
        tutorialMenu.SetActive(true);
        tutorialText.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void EndTutorial()
    {
        tutorialMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PopUp(string type)
    {
        IEnumerator myCo = Delete(raidAlert);
        if (type == "Raid"){
            raidAlert.SetActive(true);
        } else if(type == "HotspotAlert"){
            hotSpotAlert.SetActive(true);
            myCo = Delete(hotSpotAlert);
        }
        
        StartCoroutine(myCo);
    }

    IEnumerator Delete(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        go.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /*public void Fade()
    {
        Debug.Log("Recieved");
        float fadeTime = 0;
        float fadeLength = 2;
        fadePanel.gameObject.SetActive(true);
        int i = 0;
        while (fadeTime < fadeLength)
        {
            Debug.Log(i);
            fadeLength += Time.deltaTime;
            fadePanel.gameObject.GetComponent<Image>().color = new Vector4(0f, 0f, 0f, fadeTime * 122);
            i++;
        }
    }*/
    public void RemoveFade()
    {
        fadePanel.gameObject.SetActive(false);
        fadePanel.gameObject.GetComponent<Image>().color = new Vector4(0f, 0f, 0f, 0f);
    }
}
