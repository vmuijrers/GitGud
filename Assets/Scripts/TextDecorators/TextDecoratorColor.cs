using UnityEngine;
using Unity.VisualScripting;

[System.Serializable]
[CreateAssetMenu(fileName = "TextDecorator_Color")]
public class TextDecoratorColor : TextDecorator
{
    public Color color = Color.white;
    public TextDecoratorColor(string input) : base(input) { }

    public override string Decorate(string input)
    {
        string textColor = color.ToHexString();
        return @$"<color=#{textColor}>{input}</color>";
    }
}
