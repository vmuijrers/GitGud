using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using static InputSystem_Actions;

public class GameManager : MonoBehaviour
{
    private List<IClickable> clickableList = new List<IClickable>();
    private List<IUpdateable> updateableList = new List<IUpdateable>();
    [SerializeField] private ScoreBoard scoreBoard;
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        MonoBehaviour[] actors = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach(IClickable clickable in actors.OfType<IClickable>())
        {
            SubscribeClickable(clickable);
        }

        foreach (ISetupable setupable in actors.OfType<ISetupable>())
        {
            setupable.OnSetup();
        }

        foreach (IUpdateable updateable in actors.OfType<IUpdateable>())
        {
            updateableList.Add(updateable);
        }

        foreach(IScoreable scoreable in actors.OfType<IScoreable>())
        {
            scoreable.OnAddScore += scoreBoard.AddPoint;
        }

        foreach (IPlayer player in actors.OfType<IPlayer>())
        {
            player.OnDeath += ResetGame;
        }
        scoreBoard.OnScoreChanged += uiManager.OnUpdateScoreUI;
        scoreBoard.ResetScore();
    }

    // Update is called once per frame
    void Update()
    {
        updateableList.ForEach(x => x.OnUpdate());

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));
            worldPosition.y = 0;
            foreach (IClickable clickable in clickableList)
            {
                float distance = Vector3.Distance(clickable.transform.position, worldPosition);
                if (distance <= 1)
                {
                    clickable.OnClicked();
                }
            }
        }
    }

    private void ResetGame()
    {
        scoreBoard.ResetScore();
        var actors = FindObjectsByType(typeof(BaseActor), FindObjectsSortMode.None);
        foreach (BaseActor actor in actors)
        {
            actor.Respawn();
        }
    }

    public void SubscribeClickable(IClickable clickable)
    {
        clickableList.Add(clickable);
    }

    public void UnSubscribeClickable(IClickable clickable)
    {
        clickableList.Remove(clickable);
    }

    
}


public class InputHandler : MonoBehaviour, IPlayerActions
{

    public static Vector2 MoveDirection;


    InputSystem_Actions inputActions;

    public void OnAttack(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 result = context.ReadValue<Vector2>();
            MoveDirection = result;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
    }
}