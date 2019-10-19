using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Count the balls that reach the finish
/// </summary>
public class BallCounter : MonoBehaviour
{
    /// <summary>
    /// Balls that reach the car
    /// </summary>
    private List<GameObject> ballList = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ball") && !ballList.Contains(col.gameObject))
        {
            CarPlatform.Instance.IncreaseBallCount();
            col.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Clear list when new level begin
    /// </summary>
    public void ResetGame()
    {
        ballList.Clear();
    }

}
