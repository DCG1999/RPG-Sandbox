using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerScript
{
    public virtual void EnterState(PlayerStateManager playerState) { }

    public virtual void UpdateState(PlayerStateManager playerState) { }

    public virtual void ExitState(PlayerStateManager playerState) { }

}

public class Idle : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState) 
    {
        playerState.animator.SetTrigger("idle");
    }
    public override void UpdateState(PlayerStateManager playerState) 
    {
        if(playerState.moveVector.z !=0)
        {
            playerState.SwitchState(playerState.runState);
        }
    }
    public override void ExitState(PlayerStateManager playerState) 
    {
        playerState.animator.ResetTrigger("idle");
    }
}

public class Running : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState)
    {
        playerState.animator.SetTrigger("run");
    }
    public override void UpdateState(PlayerStateManager playerState) 
    {

        if(playerState.moveVector.z ==0)
        {
            playerState.SwitchState(playerState.idleState);
        }
        else
        {
            playerState.MovePlayer();
        }
    }
    public override void ExitState(PlayerStateManager playerState) 
    {
        playerState.animator.ResetTrigger("run");
    }
}

public class Sprinting : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState) {
        playerState.moveSpeed *= 2;
        playerState.animator.SetTrigger("sprint");
    }
    public override void UpdateState(PlayerStateManager playerState) {
        playerState.MovePlayer();
    }
    public override void ExitState(PlayerStateManager playerState) {
        playerState.moveSpeed /= 2;
        playerState.animator.ResetTrigger("sprint");
    }
}

public class Pickup : PlayerScript
{

    public override void EnterState(PlayerStateManager playerState)
    {
        Collider[] itemNearby = Physics.OverlapSphere(playerState.transform.position, 3f, LayerMask.GetMask("Items")); 
        Collider[] chestNearby = Physics.OverlapSphere(playerState.transform.position, 3f, LayerMask.GetMask("Chest"));
        Collider[] mapNearby = Physics.OverlapSphere(playerState.transform.position, 3f, LayerMask.GetMask("Map"));
        if (itemNearby.Length > 0)
        {
            foreach (Collider c in itemNearby)
            {
                c.GetComponent<ItemScript>().OnPickUp();
            }
        }

        if(chestNearby.Length > 0)
        {
            foreach(Collider c in chestNearby)
            {
                c.GetComponent<TreasureChestScript>().OnOpen();
            }
        }

        if(mapNearby.Length > 0)
        {
            GameObject.FindObjectOfType<HUDManager>().DisplayMap();
        }
        playerState.SwitchState(playerState.idleState);
    }
    public override void UpdateState(PlayerStateManager playerState)
    {

    }
    public override void ExitState(PlayerStateManager playerState)
    {
        
    }
}

public class CombatMode : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState)
    {
        playerState.animator.SetTrigger("drawSword");
        GameObject swordholder = GameObject.FindGameObjectWithTag("ObjectHolder");

        foreach (ItemData equippedItem in playerState.gameManager.equippedItems)
        {
            if (equippedItem.displayName.Contains("Sword"))
            {
                if (equippedItem == playerState.SwordEquipped)
                {
                    playerState.SwordEquipped.SetActive(true);
                }
                else
                {
                    if (playerState.SwordEquipped != null)
                    {
                        GameObject.Destroy(playerState.SwordEquipped);
                    }

                    GameObject sword = GameObject.Instantiate(equippedItem.prefab, swordholder.transform);
                    playerState.SwordEquipped = sword;
                }
            }
        }      
    }
    public override void UpdateState(PlayerStateManager playerState)
    {
        if (playerState.moveVector.z > 0)
        {
            playerState.SwitchState(playerState.combatMoveState);
        }
    }
    public override void ExitState(PlayerStateManager playerState)
    {
        playerState.animator.ResetTrigger("drawSword");
    }
}

public class CombatMovement : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState)
    {
       
    }
    public override void UpdateState(PlayerStateManager playerState)
    {
        if(playerState.attackInput.triggered)
        {
            playerState.SwitchState(playerState.attackState);
        }
        float movementInput = playerState.moveVector.magnitude;
        playerState.animator.SetFloat("walkSpeed", movementInput);
        playerState.MovePlayer();
    }
    public override void ExitState(PlayerStateManager playerState)
    {

    }
}

public class CombatAttack : PlayerScript
{
   // bool isAttacking;
    float timePassed;
    float animLength;
    float animSpeed;

    public override void EnterState(PlayerStateManager playerState)
    {
        playerState.isAttacking = false;
        timePassed = 0;
        playerState.animator.SetTrigger("attack");
        playerState.animator.SetFloat("walkSpeed", 0f);
    }
    public override void UpdateState(PlayerStateManager playerState)
    {
        timePassed += Time.deltaTime;
        animLength = playerState.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        animSpeed = playerState.animator.GetCurrentAnimatorStateInfo(1).speed;

        if (timePassed >= animLength / animSpeed && playerState.isAttacking)
        {
            playerState.SwitchState(playerState.attackState);
        }

        if (timePassed >= animLength/animSpeed  && !playerState.isAttacking)
        {
            playerState.SwitchState(playerState.combatMoveState);
            playerState.animator.SetTrigger("combatMove");
        }
    }
    public override void ExitState(PlayerStateManager playerState)
    {
        playerState.animator.ResetTrigger("attack");
    }
}

public class DodgeState : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState)
    {
        playerState.animator.SetTrigger("dodge");
        playerState.moveSpeed *= 2; // this value can be determined byu agility stat
        playerState.Dodge();
    }
    public override void UpdateState(PlayerStateManager playerState)
    {
        
    }
    public override void ExitState(PlayerStateManager playerState)
    {
        playerState.moveSpeed /= 2;
        playerState.animator.ResetTrigger("dodge");
        playerState.animator.SetTrigger("combatMove");
    }
}

public class CombatExit : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState)
    {
        playerState.animator.SetTrigger("sheathSword");
        playerState.SwitchModes();
    }
    public override void UpdateState(PlayerStateManager playerState)
    {

    }
    public override void ExitState(PlayerStateManager playerState)
    {
        playerState.SwordEquipped.SetActive(false);
        playerState.animator.ResetTrigger("sheathSword");
    }
}

public class DeadState : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState)
    {
        playerState.animator.SetTrigger("dead");
    }
    public override void UpdateState(PlayerStateManager playerState)
    {

    }
    public override void ExitState(PlayerStateManager playerState)
    {

    }
}

public class InteractState : PlayerScript
{
    public override void EnterState(PlayerStateManager playerState)
    {
        playerState.animator.SetTrigger("idle");
        Collider[] npcs = Physics.OverlapSphere(playerState.transform.position, 3f, LayerMask.GetMask("NPC"));
        if (npcs.Length > 0)
        {
            foreach (Collider c in npcs)
            {
                c.GetComponent<NPCScript>().Interact();
            }
        }
        else
        {
            playerState.gameManager.dialogueSystem.gameObject.SetActive(false);
            playerState.SwitchState(playerState.idleState);
            playerState.isInteracting = false;
        }
    }
    public override void UpdateState(PlayerStateManager playerState)
    {
    }
    public override void ExitState(PlayerStateManager playerState)
    {

    }
}

