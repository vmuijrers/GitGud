using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;

[CreateAssetMenu(fileName = "TextParagraph")]
public class TextParagraph : ScriptableObject
{
    [TextArea]
    public string inputText;

    public List<TextBlob> textBlobs = new List<TextBlob>();

    [ContextMenu("Parse Input To List")]
    private void ParseTextToBlobs()
    {
        var split = inputText.Split(' ');
        foreach (var s in split)
        {
            textBlobs.Add(new TextBlob(s));
        }
    }

    public string GetDecoratedText()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendJoin(' ', textBlobs.Select(x => x.GetDecoratedText()));
        return sb.ToString();
    }
}

