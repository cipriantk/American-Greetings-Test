using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject content;

    public void Show()
    {
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }
}
