using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColoringShape : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;
    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        BusSystem.Register(Topics.Base.CHANE_OBJECT_COLOR, OnChangeColorMessageReceived);
    }

    private void OnChangeColorMessageReceived(BusObject obj)
    {
        ChangeToRandomColor();
    }
    public void ChangeToRandomColor()
    {
        Color andomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        _spriteRenderer.color = andomColor;
    }
    
}
