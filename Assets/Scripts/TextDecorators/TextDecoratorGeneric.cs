using UnityEngine;

[CreateAssetMenu(fileName = "TextDecorator_Generic")]
public class TextDecoratorGeneric : TextDecorator
{
    [Tooltip("Write prefix without brackets here")]
    public string prefix = "color=blue";
    public string postfix = "/color";
    public TextDecoratorGeneric(string input) : base(input) { }

    public override string Decorate(string input)
    {
        return @$"<{prefix}>{input}<{postfix}>";
    }
}
