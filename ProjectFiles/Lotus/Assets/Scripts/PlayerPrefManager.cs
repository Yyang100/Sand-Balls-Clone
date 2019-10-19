using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set and Get the player Prefs
/// </summary>
public abstract class PlayerPrefManager 
{

    public static void SetInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }
    public static void SetFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }
    public static void SetString(string name, string value)
    {
        PlayerPrefs.SetString(name, value);
    }

    public static int GetInt(string name)
    {
        return PlayerPrefs.GetInt(name);
    }
    public static float GetFloat(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }
    public static string GetString(string name)
    {
        return PlayerPrefs.GetString(name);
    }


}
