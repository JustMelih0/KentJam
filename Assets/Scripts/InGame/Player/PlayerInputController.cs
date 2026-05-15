using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputController : MonoBehaviour
{
    public Player player;
    public PlayerStateMachine playerStateMachine;
   private bool canMove = true;

    public bool CanMove {  get { return canMove; } }

    void OnEnable()
    {
      DialogueManager.DialogueEnterEvent += DialogueEnter;
      DialogueManager.DialogueExitEvent += DialogueExit;
    }
    void OnDisable()
    {
      DialogueManager.DialogueEnterEvent -= DialogueEnter;
      DialogueManager.DialogueExitEvent -= DialogueExit;
    }
   public void DialogueEnter(DialogueSO dialogueSO)
   {
      DisControls();
   }
   public void DialogueExit(DialogueSO dialogueSO)
   {
      EnControls();
   }
   public void DisControls()
   {
      canMove = false;
      player.horizontalInput = 0;
   }
   public void EnControls()
   {
      canMove = true;
   }
    public void HorizontalInput(InputAction.CallbackContext context)
    {
      if(canMove)
      player.horizontalInput = context.ReadValue<float>();
      else
      {
         player.horizontalInput = 0;
      }
    }
   public void AttackInput(InputAction.CallbackContext context)
   {
      if (!canMove) return;

      if(context.started)
         playerStateMachine.InputRequest("AttackState");
   }
   public void JumpInput(InputAction.CallbackContext context)
   {
      if (!canMove) return;

      if(context.started)
         playerStateMachine.player_LocomotionState.InputRequest(InputType.JumpInput, context);
   }
    public void DashInput(InputAction.CallbackContext context)
    { 
       if(!canMove) return;

       if (context.started)
       {
            //playerStateMachine.player_DashState.InputRequest(InputType.DashInput, context);
       }
    }
    public void ParryInput(InputAction.CallbackContext context)
    { 
       if(!canMove) return;

      //playerStateMachine.player_ParryState.InputRequest(InputType.ParryInput, context);

    }
    public void PotDrinkInput(InputAction.CallbackContext context)
    {
       if (context.started)
       {
          SceneManager.LoadScene(0);
       }
    }
    public void InteractInput(InputAction.CallbackContext context)
    {
      if(!canMove) return;

      if (context.started)
      {
         GameManager.Instance.playerInteractController.TryInteract();
      }
    }

   


    [System.Serializable]
    public enum InputType
    {
      AttackInput,
      HorizontalInput,
      JumpInput,
      DashInput,
      ParryInput,
      PotDrinkInput
    }
}