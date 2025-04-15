using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TestScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextParagraph textParagraph;

    void Start()
    {
        
    }

    void Update()
    {
        text.text = textParagraph.GetDecoratedText();
    }
}

[System.Serializable]
public class TextBlob 
{
    public string text;
    [SerializeReference] public List<TextDecorator> decorators = new List<TextDecorator>();

    public TextBlob(string inputText)
    {
        text = inputText;
    }

    public string GetDecoratedText()
    {
        string decoratedText = text;
        foreach(var decorator in decorators)
        {
            decoratedText = decorator.Decorate(decoratedText);
        }
        return decoratedText;
    }
}

