using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    // References to relevant managers and systems//
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public InventorySystem inventorySytem;
    [HideInInspector] public CharacterClasses characterClass;
    PlayerScript player;

    // Input //
    PlayerInput playerControls;
    [HideInInspector] public InputAction attackInput;
    //--------------------------//

    [Header("Movement")]
    [HideInInspector]public Vector3 moveVector;
    Vector3 inputVector;    
    Vector3 gravityVector;
    public float moveSpeed;
    public float rotSpeed;

    // Component References //
    CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerInput playerInput;
    //--------------------------------------//

    // concrete states // 
    public Idle idleState = new Idle();
    public Running runState = new Running();
    public Sprinting sprintState = new Sprinting();
    public Pickup pickupState = new Pickup();
    public CombatMode combatState = new CombatMode();
    public CombatMovement combatMoveState = new CombatMovement();
    public CombatAttack attackState = new CombatAttack();
    public DodgeState dodgeState = new DodgeState();
    public CombatExit combatExitState = new CombatExit();
    public DeadState deadState = new DeadState();
    public InteractState interactState = new InteractState();
    // ------------------------------------------------------//

    // current state //
    public PlayerScript currentState;
    //--------------------------------//

    // bools//
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isInteracting;
    bool inventoryMode;
    bool combatMode;
    //-----------------------//

    //GameObject Ref //
    public GameObject SwordEquipped = null;
    //-------------------------------------//


    private void Awake()
    {
        isInteracting = false;
        isSprinting = false;
        isAttacking = false;

        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerControls = GetComponent<PlayerInput>();
     
        gameManager = FindObjectOfType<GameManager>();

        attackInput = playerInput.actions["attack"];

        gravityVector = new Vector3(0, -9.81f, 0);
    }

    private void Start()
    {
        Debug.Log(PlayerHealthXP.Instance.playerProperties.Health);

        currentState = idleState;
        currentState.EnterState(this);

        inventorySytem = gameManager.inventorySytem;
    }

    public void InitializePlayer()
    {
        player = new PlayerScript();
    }

    // input functions //
    private void OnMove(InputValue _inputValue)
    {
        if (IsDead()) return;
        if (isInteracting) return;
        inputVector = _inputValue.Get<Vector2>();
        moveVector.x = inputVector.x;
        moveVector.z = inputVector.y;
    }

    private void OnRun(InputValue _inputValue)
    {
        if (IsDead()) return;
        if (isInteracting) return;
        if (!combatMode)
        {
            if (_inputValue.isPressed && moveVector.magnitude != 0)
            {
                SwitchState(sprintState);
            }
            else if (!_inputValue.isPressed && moveVector.magnitude > 0)
            {
                SwitchState(runState);
            }
            else if (!_inputValue.isPressed && moveVector.magnitude == 0)
            {
                SwitchState(idleState);
            }
        }
    }

    private void OnInventory(InputValue _inventoryDisplay)
    {
        if (IsDead()) return;
        if (isInteracting) return;
        if (!combatMode)
        {
            inventoryMode = _inventoryDisplay.isPressed;
            if (inventoryMode)
            {
                gameManager.UpdateGameStateFunctions(GameManager.GameStates.INVENTORY);
            }
            else
            {
                gameManager.UpdateGameStateFunctions(GameManager.GameStates.GAME);
            }
            gameManager.DisplayInventory(inventoryMode);
        }
    }

    private void OnPickup(InputValue _pickup)
    {
        if (isInteracting) return;
        if (IsDead()) return;
        if (!combatMode && _pickup.isPressed)
        {
            SwitchState(pickupState);
        }
        
    }

    private void OnSwordAction(InputValue _swordMode)
    {
        if (isInteracting) return;
        if (IsDead()) return;
        if (_swordMode.isPressed)
        {
            combatMode = !combatMode;
        }

        if(combatMode)
        {
            SwitchState(combatState);
        }
        else
        {
            SwitchState(combatExitState);
        }
    }

    private void OnDodge(InputValue isDodging)
    {
        if (IsDead()) return;
        if (combatMode)
        {
            if (isDodging.isPressed)
            {
                SwitchState(dodgeState); 
            }
        }
    }

    private void OnAttack(InputValue _isAttacking)
    {
        if (IsDead()) return;
        isAttacking = _isAttacking.isPressed;      
    }

    private void OnTalk(InputValue _isInteracting)
    {
        if (combatMode) return;
        if(_isInteracting.isPressed)
        {
            isInteracting = !isInteracting;      
        }

        if(isInteracting)
        {
            gameManager.dialogueSystem.gameObject.SetActive(true);
            SwitchState(interactState);
        }
        else
        {
            gameManager.dialogueSystem.gameObject.SetActive(false);
            SwitchState(idleState);
        }
    }
    //------------------------------------------------------------------//


    private void Update()
    {
        ApplyGravity();
        currentState.UpdateState(this);

        if (!IsDead())
        {
            RotatePlayer();
        }
    }

    public void SwitchState(PlayerScript newState)
    {
        currentState.ExitState(this);

        currentState = newState;
        newState.EnterState(this);  
    }

    // Movement methods//
    public void MovePlayer()
    {
        Vector3 moveDir = transform.forward * moveVector.z;
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    public void ApplyGravity()
    {
        if (GameManager.gameState == GameManager.GameStates.GAME)
        {
            characterController.Move(gravityVector * Time.deltaTime);
        }
        else { return; }
    }

    public void RotatePlayer()
    {
        if (moveVector.x == 0) return;
        transform.RotateAround(transform.position, Vector3.up, moveVector.x * rotSpeed * Time.deltaTime);
    }

    public void Dodge() => StartCoroutine(PlayerDodge());

    private IEnumerator PlayerDodge()
    {
        float startTime = Time.time;
        float dashTime = 0.25f; // can change this based on agility stats
        while(Time.time < startTime + dashTime)
        {
            characterController.Move(-transform.forward * moveSpeed * Time.deltaTime);
            yield return null;
        }
        SwitchState(combatMoveState);
        yield return new WaitForEndOfFrame();
    }
    //--------------------------------------------------------------------------------------//


    //transition between combat to loco//
    public void SwitchModes() => StartCoroutine(SwitchFromCombatToLoco());

    private IEnumerator SwitchFromCombatToLoco()
    {
        yield return new WaitForSeconds(0.8f); // generic value for sheath animation to play
        SwitchState(idleState);
    }
    //------------------------------------------------------------------------------------//


    public void InitiateDamage()
    {
      
        SwordEquipped.GetComponent<MeleeScript>().InitiateDamage();
    }
    public void EndInflictingDamage()
    {
        SwordEquipped.GetComponent<MeleeScript>().EndInflictingDamage();
    }

    public bool IsDead()
    {
        if (PlayerHealthXP.Instance.playerProperties.Health <= 0) return true;
        else return false;
    }

    public void TakeDamage(float damage)
    {
        if (!IsDead())
        {
            PlayerHealthXP.Instance.playerProperties.Health -= damage;
            Debug.Log("Player Health : " + PlayerHealthXP.Instance.playerProperties.Health);
            FindObjectOfType<HUDManager>().healthBar.value = PlayerHealthXP.Instance.playerProperties.Health;
            if (IsDead())
            {
                gameManager.UpdateGameStateFunctions(GameManager.GameStates.LOSE);
                SwitchState(deadState);
            }
        }
    }
}