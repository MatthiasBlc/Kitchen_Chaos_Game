using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
  public static Player Instance { get; private set; }

  public event EventHandler OnPickedSometing;


  public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
  public class OnSelectedCounterChangedEventArgs : EventArgs
  {
    public BaseCounter selectedCounter;
  }

  [SerializeField] private float moveSpeed = 7f;
  [SerializeField] private GameInput gameInput;
  [SerializeField] private LayerMask countersLayerMask;
  [SerializeField] private Transform kitchenObjectHoldPoint;


  private bool isWalking;
  private Vector3 lastInteractDir;
  private BaseCounter selectedCounter;
  private KitchenObject kitchenObject;



  private void Awake()
  {
    if (Instance != null)
    {
      Debug.LogError("There is more than one Player instance");
    }
    Instance = this;
  }

  private void Start()
  {
    gameInput.OnInteractAction += GameInput_OnInteractAction;
    gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
  }

  private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
  {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;

    if (selectedCounter != null)
    {
      selectedCounter.InteractAlternate(this);
    }
  }

  private void GameInput_OnInteractAction(object sender, System.EventArgs e)
  {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;

    if (selectedCounter != null)
    {
      selectedCounter.Interact(this);
    }
  }

  private void Update()
  {
    HandleMovement();
    HandleInterractions();
  }

  public bool IsWalking()
  {
    return isWalking;
  }

  private void HandleInterractions()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    if (moveDir != Vector3.zero)
    {
      lastInteractDir = moveDir;
    }

    float interractDistance = 2f;

    if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interractDistance, countersLayerMask))
    {
      if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
      // is same as :
      // ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
      // if (clearCounter !=null)
      // {
      //Has ClearCounter
      // }
      {
        //Has ClearCounter
        if (baseCounter != selectedCounter)
        {
          SetSelectedCounter(baseCounter);
        }
      }
      else
      {
        SetSelectedCounter(null);
      }
    }
    else
    {
      SetSelectedCounter(null);
    }
  }

  private void HandleMovement()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    float moveDistance = moveSpeed * Time.deltaTime;
    float playerRadius = .7f;
    float playerHeight = 2f;

    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

    if (!canMove)
    {
      //Cannot move toward moveDir

      // Attempt only X movement || .normalized allow the speed to be full
      Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
      canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

      if (canMove)
      {
        // Can move only on the x 
        moveDir = moveDirX;
      }
      else
      {
        // Cannot move only on the x

        // Attempt only Z movement
        Vector3 moveDirZ = -new Vector3(0, 0, moveDir.z).normalized;
        canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
        if (canMove)
        {
          // Can move only on the z
          moveDir = moveDirZ;
        }
        else
        {
          // Cannot move in any direction
        }

      }
    }

    if (canMove)
    {
      transform.position += moveDir * moveDistance;
    }


    isWalking = moveDir != Vector3.zero;
    float rotateSpeed = 7f;
    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
  }

  private void SetSelectedCounter(BaseCounter selectedCounter)
  {
    this.selectedCounter = selectedCounter;

    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
    {
      selectedCounter = selectedCounter
    });
  }

  // Functions for the interface
  public Transform GetKitchenObjectFollowTransform()
  {
    return kitchenObjectHoldPoint;
  }

  public void SetKitchenObject(KitchenObject kitchenObject)
  {
    this.kitchenObject = kitchenObject;

    if (kitchenObject != null)
    {
      OnPickedSometing?.Invoke(this, EventArgs.Empty);
    }
  }

  public KitchenObject GetKitchenObject()
  {
    return kitchenObject;
  }

  public void ClearKitchenObject()
  {
    kitchenObject = null;
  }

  public bool HasKitchenObject()
  {
    return kitchenObject != null;
  }
}


