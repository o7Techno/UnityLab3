using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameActions))]
public class InputManager : MonoBehaviour
{
    public static int movingCellAmount { get; set; }

    GameActions actions;

    const float swipeThreshold = 50f;

    InputAction upAction;
    InputAction downAction;
    InputAction leftAction;
    InputAction rightAction;

    InputAction swipeAction;
    Vector2 swipeStartPos;

    private void Awake()
    {
        movingCellAmount = 0;
        actions = GetComponent<GameActions>();
        //if (!TryGetComponent<GameActions>(out actions))
        //{
        //    Debug.LogError("Game Actions not found.");
        //}

        upAction = new InputAction("Up");
        upAction.AddBinding("<Keyboard>/w");
        upAction.AddBinding("<Keyboard>/upArrow");

        downAction = new InputAction("Down");
        downAction.AddBinding("<Keyboard>/s");
        downAction.AddBinding("<Keyboard>/downArrow");

        leftAction = new InputAction("Left");
        leftAction.AddBinding("<Keyboard>/a");
        leftAction.AddBinding("<Keyboard>/leftArrow");

        rightAction = new InputAction("Right");
        rightAction.AddBinding("<Keyboard>/d");
        rightAction.AddBinding("<Keyboard>/rightArrow");

        upAction.performed += ctx => HandleInput(() => { Debug.Log("Up input"); actions.Up(); });
        downAction.performed += ctx => HandleInput(() => { Debug.Log("Down input"); actions.Down(); });
        leftAction.performed += ctx => HandleInput(() => { Debug.Log("Left input"); actions.Left(); });
        rightAction.performed += ctx => HandleInput(() => { Debug.Log("Right input"); actions.Right(); });

        swipeAction = new InputAction("Swipe", InputActionType.Button);
        swipeAction.AddBinding("<Mouse>/leftButton");
        swipeAction.AddBinding("<Touchscreen>/primaryTouch/press");
    }

    private void OnEnable()
    {
        upAction.Enable();
        downAction.Enable();
        leftAction.Enable();
        rightAction.Enable();
        swipeAction.Enable();
    }

    private void OnDisable()
    {
        upAction.performed -= ctx => HandleInput(() => { Debug.Log("Up input"); actions.Up(); });
        downAction.performed -= ctx => HandleInput(() => { Debug.Log("Down input"); actions.Down(); });
        leftAction.performed -= ctx => HandleInput(() => { Debug.Log("Left input"); actions.Left(); });
        rightAction.performed -= ctx => HandleInput(() => { Debug.Log("Right input"); actions.Right(); });

        upAction.Disable();
        downAction.Disable();
        leftAction.Disable();
        rightAction.Disable();
        swipeAction.Disable();
    }

    void HandleInput(System.Action actionMethod)
    {
        if (movingCellAmount == 0)
        {
            actionMethod?.Invoke();
        }
    }
}
