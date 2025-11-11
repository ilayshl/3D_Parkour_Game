using System;
using TMPro;
using UnityEngine;

/// <summary>
/// A temporary script to display the necessary debug information.
/// </summary>
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedValueText, stateValueText, mouseSensitivity;
    [SerializeField] private PlayerManager manager;
    [SerializeField] private PlayerCamera playerCamera;

    void LateUpdate()
    {
        speedValueText.SetText(Math.Round(manager.CurrentVelocity.magnitude, 3).ToString());
        mouseSensitivity.SetText(playerCamera.sensitivityX.ToString());
        stateValueText.SetText(manager.CurrentState.ToString());
    }

}
