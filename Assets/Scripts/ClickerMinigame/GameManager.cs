using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    private List<IClickable> clickableList = new List<IClickable>();
    private List<IUpdateable> updateableList = new List<IUpdateable>();
    [SerializeField] private ScoreBoard scoreBoard;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Player player;

    private void Start()
    {
        MonoBehaviour[] actors = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        DoFunction((x) => x.OnSetup(), actors.OfType<ISetupable>());

        DoFunction((x) => SubscribeClickable(x), Registry<IClickable>.Items);
        DoFunction((x) => x.SetTarget(player), Registry<ITargetUser>.Items);
        DoFunction((x) => updateableList.Add(x), Registry<IUpdateable>.Items);
        DoFunction((x) => x.OnAddScore += scoreBoard.AddPoint, Registry<IScoreable>.Items);
        DoFunction((x) => x.OnDeath += ResetGame, Registry<IPlayer>.Items);

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

    private void DoFunction<T>(System.Action<T> action, IEnumerable<T> input)
    {
        foreach(T item in input)
        {
            action?.Invoke(item);
        }
    }

    private void ResetGame()
    {
        Debug.Log("Game Restarted!");
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

public static class VectorExtensions
{
    public static Vector3 WithXYZ(this Vector3 vec, float? x = null, float? y = null, float? z = null)
    {
        vec = new Vector3(x ?? vec.x, y ?? vec.y, z ?? vec.z);
        return vec;
    }
}

public static class Registry<T>
{
    private static HashSet<T> set = new HashSet<T>();
    public static IEnumerable<T> Items => set;

    public static bool ItemExists(T item) => set.Contains(item);
    
    public static void Register(T item)
    {
        set.Add(item);
    }

    public static void UnRegister(T item)
    {
        set.Remove(item);
    }

    public static IEnumerable<T> FilterItems(Func<T, bool> condition)
    {
        return set.Where(condition);
    }
}