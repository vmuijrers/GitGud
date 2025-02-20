using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour, ISetupable
{
    private Label scoreLabel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSetup()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        scoreLabel = new Label("Score: 0");
        root.Add(scoreLabel);
    }
    
    public void OnUpdateScoreUI(int points)
    {
        scoreLabel.text = "Score: " + points.ToString();
    }
}
