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
    private TextMeshProUGUI time;
    [SerializeField]
    private TextMeshProUGUI bonus;
    [SerializeField]
    private TextMeshProUGUI final_score;
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

        yield return null;
    }
}
