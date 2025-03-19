using UnityEngine;

public class MainLight : MonoBehaviour
{
    [SerializeField]
    private Material material;

    void Update()
    {
        material.SetVector("_LightDirection", transform.forward);
    }
}
