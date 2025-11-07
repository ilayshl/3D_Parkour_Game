using System;
using TMPro;
using UnityEngine;

/// <summary>
/// A Temporary script to display the necessary debug information.
/// </summary>
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedValueText, stateValueText, mouseSensitivity;
    [SerializeField] private PlayerManager manager;
    [SerializeField] private PlayerCamera playerCamera;

    void OnEnable()
    {
        //playerManager.OnMovementStateChanged += UpdateStateText;
    }

    void OnDisable()
    {
        //playerManager.OnMovementStateChanged -= UpdateStateText;
    }

    void LateUpdate()
    {
        speedValueText.SetText(Math.Round(manager.CurrentVelocity.magnitude, 3).ToString());
        mouseSensitivity.SetText(playerCamera.sensitivityX.ToString());
        stateValueText.SetText(manager.CurrentState.ToString());

        /* if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            playerCamera.sensitivityX -= 25;
            playerCamera.sensitivityY -= 25;
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            playerCamera.sensitivityX += 25;
            playerCamera.sensitivityY += 25;
        } */

    }

}
