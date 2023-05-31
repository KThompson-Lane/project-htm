using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    public Sprite full, half, empty;

    private Image _containerImage;
    void Awake()
    {
        _containerImage = GetComponent<Image>();
    }

    public void UpdateContainer(HeartStatus status)
    {
        _containerImage.sprite = status switch
        {
            HeartStatus.Full => full,
            HeartStatus.Half => half,
            _ => empty
        };
    }
}

public enum HeartStatus
{
    Empty,
    Half,
    Full,
}
