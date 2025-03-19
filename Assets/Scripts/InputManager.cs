using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameObject _clickedObject;

    private float _doubleCLickTimeCounter;

    private float _doubleClickTimer = 0.2f;

    private void Awake()
    {
        BusSystem.Register(Topics.Base.CHANGE_TO_PREVIOUS_SHAPE, OnChangePreviousShape);
        BusSystem.Register(Topics.Base.CHANGE_TO_NEXT_SHAPE, OnChangeNextShape);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.transform != null)
            {
                if (_clickedObject == hit.transform.gameObject)
                {
                    OnDoubleClick(hit.transform.gameObject);
                    _clickedObject = null;
                }
                else if (hit.collider != null)
                {
                    _clickedObject = hit.transform.gameObject;
                    _doubleClickTimer = 0.2f;
                }
            }
        }

        _doubleClickTimer -= Time.deltaTime;
        if (_doubleClickTimer < 0)
        {
            _clickedObject = null;
        }
    }

    void OnDoubleClick(GameObject clickedObject)
    {
        BusSystem.Execute(Topics.Base.CHANE_OBJECT_COLOR, new BusObject() { Content = clickedObject });
        BusSystem.Execute(Topics.Audio.PLAY_SFX, new BusObject() {Content = new SfxInData() { SfxEnum = AudioEnums.SfxEnum.SFX_Double_click, StopSfxWithSameName = true}});
    }

    void OnChangePreviousShape(BusObject busObject)
    {
        _clickedObject = null;
    }
    
    void OnChangeNextShape(BusObject busObject)
    {
        _clickedObject = null;
    }
}