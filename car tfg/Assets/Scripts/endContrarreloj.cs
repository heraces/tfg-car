using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class endContrarreloj : MonoBehaviour
{
    private Animator thisAnimator;
    [SerializeField]
    private TextMeshProUGUI controlPoints;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI bonus;
    [SerializeField]
    private TextMeshProUGUI final_score;

    private const float addSpeed = 50f;
    private float finalTimeVar;
    private float bonusScore;
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }

    public void callMe( float finalTime, int controlPointsAchieved, int totalControlPoints)
    {
        StartCoroutine(UpItUp(finalTime, controlPointsAchieved, totalControlPoints));
        thisAnimator.Play("final_score");
    }

    private IEnumerator UpItUp(float finalTime, int controlPointsAchieved, int totalControlPoints)
    {
        finalTimeVar = 0;
        controlPoints.text = "Controls: " + controlPointsAchieved.ToString() + "/" + totalControlPoints.ToString();
        bonusScore = 1 + 3 * Mathf.Pow(controlPointsAchieved / totalControlPoints, 2);

        while (finalTimeVar < finalTime)
        {
            finalTimeVar += addSpeed * Time.deltaTime;
            timeText.text = "Time:  " + finalTimeVar.ToString();
            bonus.text = "\t + " + bonusScore.ToString();
            final_score.text = "Score:  " + (finalTimeVar * bonusScore).ToString();
            yield return null;
        }
    }
}
