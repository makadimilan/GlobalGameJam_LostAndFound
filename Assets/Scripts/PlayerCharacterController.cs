using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] int rewiredPlayerId = 0;
    [SerializeField] string moveAxisName = "Move";
    [SerializeField] string jumpButtonName = "Jump";

    [SerializeField, HideInInspector] CharacterMovement _characterMovement = null;
    public CharacterMovement CharacterMovement
    {
        get
        {
            if (_characterMovement == null)
            {
                _characterMovement = GetComponent<CharacterMovement>();
            }

            return _characterMovement;
        }
    }
    
    Rewired.Player rewiredPlayer = null;
    void Start()
    {
        rewiredPlayer = Rewired.ReInput.players.GetPlayer(rewiredPlayerId);
    }

    void Update()
    {
        CharacterMovement.Move(rewiredPlayer.GetAxis(moveAxisName));

        if (rewiredPlayer.GetButtonDown(jumpButtonName))
        {
            CharacterMovement.Jump();
        }
    }
}
