using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] int rewiredPlayerId = 0;
    [SerializeField] string moveAxisName = "Move";
    [SerializeField] string horizontalAimAxisName = "AimX";
    [SerializeField] string verticalAimAxisName = "AimY";
    [SerializeField] string jumpButtonName = "Jump";
    [SerializeField] string grabButtonName = "Grab";
    [SerializeField] string mouseAimButtonName = "MouseAim";

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
        CharacterMovement.SetHandGrab(rewiredPlayer.GetButton(grabButtonName));

        if (rewiredPlayer.GetButtonDown(jumpButtonName))
        {
            CharacterMovement.Jump();
        }

        if (rewiredPlayer.GetButton(mouseAimButtonName))
        {
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            CharacterMovement.SetArmTarget(worldPosition - (Vector2)transform.position);
        }
        else
        {
            CharacterMovement.SetArmTarget(new Vector2(rewiredPlayer.GetAxis(horizontalAimAxisName), rewiredPlayer.GetAxis(verticalAimAxisName)));
        }
    }

    void OnDrawGizmos() 
    {
        if (rewiredPlayer != null && rewiredPlayer.GetButton(mouseAimButtonName))
        {
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);
            Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(screenPosition), 0.1f);
        }
    }
}
