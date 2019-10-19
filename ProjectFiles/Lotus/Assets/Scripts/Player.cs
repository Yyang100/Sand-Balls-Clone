using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make a path with mouse positions
/// </summary>
public class Player : MonoBehaviour
{
    public float _radius = 1f;

    private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }

    private bool _isMoving;

    private bool _canMove = true; 
  

    /// <summary>
    /// Called from InputManager.
    /// Change the path/walls
    /// </summary>
    /// <param name="position"></param>
    public void UpdateThePath(Vector3 position)
    {
        if (_canMove)
        {
            transform.position = position;
            foreach (RegionGenerator regionGenerator in RegionManager.Instance._activeRegions)
            {
                regionGenerator.Cut(position, _radius);
            }
        }
    }
     
    public void CanMove(bool value)
    {
        _canMove = value;
    }
}
