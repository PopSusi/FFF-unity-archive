using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeatGauge : MonoBehaviour
{
    public static int heat = 50;
    [SerializeField] private int maxMargin = 20;
    public static float currMargin;

    [SerializeField] private int coldThreshold = 40;
    [SerializeField] private int heatThreshold = 60;

    private int hotspotCount;
    List<GameObject> hotspots = new List<GameObject>();

    public static int firefightersCount;

    [SerializeField] int passive;

    public static float timeF;
    public static int time;

    public static HeatGauge instance;
    static bool pausedTimer;
    public enum LevelNum
    {
        first, second, third

    }
    public LevelNum myLevel;
    private void Awake()
    {
        heat = 50;
        pausedTimer = false;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        StartCoroutine("Passive");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!pausedTimer)
        {
            timeF += Time.deltaTime;
            time = (int)timeF;
            if (timeF > 300)
            {
                UIManager.instance.Status("win");
            }
            if ((time % 60) == 0 && time != 0)
            {
                pausedTimer = true;
                StartCoroutine("Fade");

            }
        }
        //Debug.Log($"time is {timeF}, stage is {(time % 60)}");
    }
    public static void TickTemperature(int degrees, int passive)
    {
        heat += degrees;
        passive+= passive;
        CheckForLoss();
        //Debug.Log("temp");
    }

    void CheckPassive()
    {
        if (heat < coldThreshold)
        {
            passive = -2;
        }
        else if (heat <= coldThreshold && heat >= heatThreshold)
        {
            passive = 0;
        }
        else
        {
            passive = 2;
        }
        CheckForLoss();
    }
    private static void CheckForLoss()
    {
        if(heat <= 0)
        {
            UIManager.instance.Status("tooCold");
        } else if (heat >= 100)
        {
            UIManager.instance.Status("tooHot");
        }
    }

    IEnumerator Passive()
    {
        CheckPassive();
        heat += passive;
        currMargin = (float) (Mathf.Abs(heat - 50) / (50 / maxMargin)) / 100;
        //Debug.Log($"Heat is at {heat}, chances fluctuate by {currMargin}, passive is at {passive}");
        yield return new WaitForSeconds(3f);
        StartCoroutine("Passive");
    }

    public void HotspotSpawn() => TickTemperature(8, 1);
    public void RaidSpawn() => TickTemperature(-8, -1);
    public void RaidClear() => TickTemperature(0, 1);
    public void HotspotClearPlayer() => TickTemperature(-15, -1);
    public void FiremanDead() => TickTemperature(1, 0);

    public void NextLevel()
    {
        if (myLevel == LevelNum.first)
        {
            SceneManager.LoadScene("LevelTwo");
        }
        else if (myLevel == LevelNum.second)
        {
            SceneManager.LoadScene("LevelThree");
        }
        else
        {
            UIManager.instance.Status("win");
        }
    }
    public IEnumerator Fade()
    {
        Debug.Log("Fading");
        //UIManager.instance.Fade();
        yield return new WaitForSeconds(2f);
        NextLevel();
        Debug.Log("Faded");
    }



}
