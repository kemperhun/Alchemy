using UnityEngine;
using UnityEngine.UI;

public class MenuWorldCard : MonoBehaviour
{
    [SerializeField]
    Text WorldName;
    [SerializeField]
    Text TotalPlayedTime;
    [SerializeField]
    Text TotalGeneration;

    public void SetWorldName(string text)
    {
        WorldName.text = text;
    }
    public void SetTotalPlayedTime(string text)
    {
        TotalPlayedTime.text = text;
    }
    public void SetTotalGeneration(string text)
    {
        TotalGeneration.text = text;
    }
}
