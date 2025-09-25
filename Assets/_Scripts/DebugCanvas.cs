using System;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedValueText, stateValueText;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerManager playerManager;

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
        speedValueText.SetText(Math.Round(player.currentVelocity.magnitude, 3).ToString());
    }

    private void UpdateStateText(MovementState state)
    {
        stateValueText.SetText(state.ToString());
    }
}
