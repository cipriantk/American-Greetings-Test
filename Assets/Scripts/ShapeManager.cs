using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{

    private int _shapeIndex = 0;
    private bool _animating = false;
    
    [SerializeField] private Material wiggleMaterial;

    private Vector3 _originalPosition;
    private Vector3 _originalScale;
    
    
    private const string _Influence = "_Influence";
    
    private void Awake()
    {
        BusSystem.Register(Topics.Base.CHANE_OBJECT_COLOR, OnChangeObjectColor);
        BusSystem.Register(Topics.Base.CHANGE_TO_PREVIOUS_SHAPE, OnChangeToPreviousShape);
        BusSystem.Register(Topics.Base.CHANGE_TO_NEXT_SHAPE, OnChangeToNextShape);
        BusSystem.Register(Topics.Base.COMPLEX_ANIMATE, OnComplexAnimate);
        BusSystem.Register(Topics.Base.TOGGLE_SHADER, OnToggleShader);
        
        
        BusSystem.Register(Topics.Game.START_GAME, OnStartGame);
        BusSystem.Register(Topics.Game.END_GAME, OnEndGame);
    }

    private void Start()
    {
        wiggleMaterial.SetFloat(_Influence ,0);
        _originalPosition = transform.position;
        _originalScale = transform.localScale;
    }

    void OnChangeObjectColor(BusObject busObject)
    {
        if (busObject.Content is GameObject busGameObject)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject == busGameObject)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    _shapeIndex = i;

                    StartCoroutine(AnimateShapeOnCLick());
                }
                else
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }



    private void OnChangeToPreviousShape(BusObject busObject)
    {
        transform.GetChild(_shapeIndex).gameObject.SetActive(false);

        _shapeIndex -= 1;

        if (_shapeIndex < 0)
        {
            _shapeIndex = transform.childCount - 1;
        }

        transform.GetChild(_shapeIndex).gameObject.SetActive(true);
    }

    private void OnChangeToNextShape(BusObject busObject)
    {
        transform.GetChild(_shapeIndex).gameObject.SetActive(false);

        _shapeIndex += 1;

        if (_shapeIndex > transform.childCount - 1)
        {
            _shapeIndex = 0;
        }

        transform.GetChild(_shapeIndex).gameObject.SetActive(true);
    }

    IEnumerator AnimateShapeOnCLick()
    {
        float elapseTime = 0f;
        float waitTime = 0.4f;

        Vector3 originalLocalPosition = transform.GetChild(_shapeIndex).localPosition;;

        while (elapseTime < waitTime)
        {
            float ratio = elapseTime / waitTime;

            transform.GetChild(_shapeIndex).localPosition =
                originalLocalPosition + new Vector3(0, Mathf.Sin(Time.time * 40) * 0.2f, 0);

            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.GetChild(_shapeIndex).localPosition = originalLocalPosition;
    }
    
    
    void OnToggleShader(BusObject busObject)
    {
        float influence = wiggleMaterial.GetFloat(_Influence);

        if (influence < 0.01f)
        {
            wiggleMaterial.SetFloat(_Influence ,1);
        }
        else
        {
            wiggleMaterial.SetFloat(_Influence ,0);
        }
    }
    

    void OnComplexAnimate(BusObject busObject)
    {
        if (_animating == false)
        {
            StartCoroutine(ComplexAnimationCoroutine());
        }
    }
    
    void OnStartGame(BusObject busObject)
    {
        transform.localScale = 0.5f * Vector3.one;
    }
    
    void OnEndGame(BusObject busObject)
    {
        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }
    
    
    IEnumerator ComplexAnimationCoroutine()
    {
        _animating = true;
        
        Camera cachedCamera = Camera.main;
        Vector3 originalPosition = transform.position;


        List<Vector3> positions = new List<Vector3>();
        positions.Add(cachedCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 2)));
        positions.Add(cachedCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 2)));
        positions.Add(cachedCamera.ScreenToWorldPoint(new Vector3(0, 0, 20)));
        positions.Add(cachedCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 2)));
        positions.Add(originalPosition);
        
        BusSystem.Execute(Topics.Audio.PLAY_SFX, new BusObject() {Content = new SfxInData() { SfxEnum = AudioEnums.SfxEnum.SFX_Animate, StopSfxWithSameName = true}});
        
        for (int i = 0; i < positions.Count; i++)
        {
            float elapseTime = 0f;
            float waitTime = 1f;
            
            while (elapseTime < waitTime)
            {
                float ratio = elapseTime / waitTime;
                transform.position = Vector3.Lerp(originalPosition, positions[i], ratio);

                elapseTime += Time.deltaTime;
                yield return null;
            }

            originalPosition = transform.position;
        }

        _animating = false;
    }
}