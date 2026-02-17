using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour, ISetupable
{
    private Label scoreLabel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSetup()
    {
        Debug.Log("Adding label to UI");
        UIDocument uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        scoreLabel = root.Q<Label>("Score");
        //scoreLabel = new Label("Score1");
        //root.Add(scoreLabel);
        scoreLabel.text = "Score: 0";
    }
    
    public void OnUpdateScoreUI(int points)
    {
        scoreLabel.text = "Score: " + points.ToString();
    }
}
