using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
/// <summary>
/// Follow the ball that at the bottom
/// </summary>
public class CameraController : MonoBehaviour
{
    public float smooth = 0.5f;

    public float offset;
     
    private Vector3 finishPos;
      
    private Vector3 velocity;

    private List<GameObject> balls = new List<GameObject>();

    private BallManager ballManager;

    [Inject]
    private void Initialize(BallManager ballManager)
    {
        this.ballManager = ballManager;
    }

    /// <summary>
    /// Find the ball at the bottom
    /// </summary>
     public Transform LowestBall
    {
        get
        {
            var lowest = (from t in balls orderby t.transform.position.y select t).ToList()[0];
 
            return lowest.transform;
        }
        set { }
    }

    /// <summary>
    /// Start folling when game ready
    /// </summary>
    /// <param name="finishPosition"></param>
    public void StartFollowing(Vector3 finishPosition)
    {
        balls = ballManager.ballPoolList;
        finishPos = finishPosition;
    }

    /// <summary>
    /// Follow the ball
    /// </summary>
    void LateUpdate()
    {
        if (this.LowestBall != null)
        {
            Vector3 vector = new Vector3(0f, this.LowestBall.position.y + this.offset, -10f);
            if (vector.y < transform.position.y)
            {
                transform.position = Vector3.SmoothDamp(transform.position, vector, ref this.velocity, this.smooth);
            }
        }
    }

    /// <summary>
    /// Reset the camera
    /// </summary>
    public void ResetCamera()
    {
        transform.position = new Vector3(0f, this.LowestBall.position.y + this.offset, -10f);
    }
}
