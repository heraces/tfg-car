using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class countDown : MonoBehaviour
{
    private float targetTime = 7;
    private float addOn = 7;

    [SerializeField]
    private TextMeshProUGUI text;
    private Engine player;

    private void Start()
    {
        stopCounting();
        player = FindObjectOfType<Engine>();
    }

    void Update()
    {
        if (targetTime > 0)
        {
            targetTime -= Time.deltaTime;
            text.text = targetTime.ToString("F3") + "'";
        }
        else
        {
            timeRunOut();
        }
    }

    private void timeRunOut()
    {
        player.dontRun();
        stopCounting();
    }
    public void AddTime()
    {
        targetTime += addOn;
    }

    public void startCounting()
    {
        text.gameObject.SetActive(true);
        targetTime = addOn;
    }
    public void stopCounting()
    {
        text.gameObject.SetActive(false);
    }
}
