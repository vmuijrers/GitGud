using UnityEngine;

public abstract class TextDecorator : ScriptableObject
{
    protected string baseString = "";
    public string enhancedString;
    public TextDecorator(string input)
    {
        baseString = input;
        enhancedString = Decorate(baseString);
    }

    public abstract string Decorate(string input);
}
