using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Manage the region Generators
/// </summary>
public class RegionManager : Singleton<RegionManager>
{ 
 
    /// <summary>
    /// using for create maps/paths
    /// </summary>
    [HideInInspector]
    public List<RegionGenerator> _activeRegions = new List<RegionGenerator>();

    public float _floorGap = 5.5f;

    private RegionGenerator[] _regions;

    private float _detectionRadius;

    private Player _player;


    [Inject]
    private void Installer(Player player)
    {
        _player = player;
    }

    /// <summary>
    /// Find regionGenerators
    /// </summary>
    public void SetRegions()
    {
        _regions = FindObjectsOfType<RegionGenerator>();
        _detectionRadius = _floorGap - _player._radius;
    }

    /// <summary>
    /// Reset the regionGenerators
    /// reset the map / view
    /// </summary>
    public void ResetRegions()
    {
        _activeRegions.Clear();
        foreach (RegionGenerator regionGenerator in _regions)
        {
            regionGenerator.InialitizeMap();
        }
        SetRegions();
    }

    private void Update()
    {
        SetActiveRegions();
    }

    /// <summary>
    /// if a map is at too high remove it from the list
    /// </summary>
    private void SetActiveRegions()
    {
        foreach (RegionGenerator regionGenerator in _regions)
        {
            if (Mathf.Abs(regionGenerator.transform.position.y - _player.transform.position.y) <= _detectionRadius)
            {
                if (!_activeRegions.Contains(regionGenerator))
                {
                    _activeRegions.Add(regionGenerator);
                }
            }
            else if (_activeRegions.Contains(regionGenerator))
            {
                _activeRegions.Remove(regionGenerator);
            }
        }
    }


}
