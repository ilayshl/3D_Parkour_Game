using System;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedValueText, stateValueText, mouseSensitivity;
    //[SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerCamera playerCamera;

    void OnEnable()
    {
        playerManager.OnMovementStateChanged += UpdateStateText;
    }

    void OnDisable()
    {
        playerManager.OnMovementStateChanged -= UpdateStateText;
    }

    void LateUpdate()
    {
        //speedValueText.SetText(Math.Round(player.CurrentVelocity.magnitude, 3).ToString());
        mouseSensitivity.SetText(playerCamera.sensitivityX.ToString());

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

    private void UpdateStateText(MovementState state)
    {
        stateValueText.SetText(state.ToString());
    }
}
