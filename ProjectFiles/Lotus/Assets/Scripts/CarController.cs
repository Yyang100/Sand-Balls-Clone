using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the car/truck
/// </summary>
public class CarController : MonoBehaviour
{
    /// <summary>
    /// Target Position
    /// </summary>
    public Transform _target;
    /// <summary>
    /// Start Position
    /// </summary>
    public Transform startTransform;
    
    /// <summary>
    /// SmoothDamp variables
    /// </summary>
    private Vector3 velocity;
    public float smooth= 0.5f;


    public void Move(Vector3 targetPos)
    {
        StartCoroutine(MoveCar(targetPos));
    }

    public void Move()
    {
        StartCoroutine(MoveCar(_target.position));
    }

    /// <summary>
    /// Move the car from start to target
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    IEnumerator MoveCar(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, _target.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref velocity, smooth);
            yield return null;
        }
    }

    /// <summary>
    /// Reset the car (position)
    /// </summary>
    public void ResetCar()
    {
        StopAllCoroutines();
        transform.position = startTransform.position;
    }


}
