using UnityEngine;

[CreateAssetMenu(fileName = "TextDecorator_Bounce")]
public class TextDecoratorBounce : TextDecorator
{
    public float pixelBounceAmount = 5;
    public float frequency = 1f;
    private float time = 0;
    private float curBounceAmount = 0;
    public TextDecoratorBounce(string input) : base(input) { }

    public override string Decorate(string input)
    {
        time += Time.deltaTime * frequency;
        curBounceAmount = pixelBounceAmount * Mathf.Sin(time);
        return $"<voffset={curBounceAmount}px>{input}</voffset>";
    }
}