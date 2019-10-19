using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
/// <summary>
/// Get mouse positon and send player to create path
/// </summary>
public class InputManager : MonoBehaviour
{

    private Player player;
    private Camera mainCamera;

    [Inject]
    private void Installer(Player player)
    {
        this.player = player;
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
            player.UpdateThePath(position);
        }
    }


}
