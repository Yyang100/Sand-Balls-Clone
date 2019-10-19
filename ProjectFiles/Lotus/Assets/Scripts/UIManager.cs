using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Manage the UI
/// open close stageComplete panel
/// button action methods
/// </summary>
public class UIManager : MonoBehaviour
{

    public GameObject stageClear;
    public GameEvent restartGameEvent;
    private LevelManager levelManager;

    [Inject]
    private void Installer(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }


    /// <summary>
    /// Open the level completed /Stage Clear panel
    /// </summary>
    public void OpenStageClearPanel()
    {
        stageClear.SetActive(true);
    }

    /// <summary>
    /// Close the level completed /Stage Clear panel
    /// </summary>
    public void ResetUI()
    {
        stageClear.SetActive(false);
    }


    /// <summary>
    /// Restart the level when clicked
    /// </summary>
    public void RestartButton()
    {
        levelManager.RestartLevel();
        restartGameEvent.Raise();
    }

    /// <summary>
    /// Load the new level when cliked
    /// </summary>
    public void NextLevelButton()
    {
        levelManager.LoadNextLevel();
        restartGameEvent.Raise();
    }


}
