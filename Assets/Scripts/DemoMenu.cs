using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class DemoMenu : Menu
{
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
    [Space]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button complexAnimateButton;
    [SerializeField] private Button toggleShader;
    
    [Space]
    [SerializeField] private Button startGame;

    [Space] [SerializeField] private GameObject gameInstructions;
    
    [Space] [SerializeField] private Delays delays;
    private void Awake()
    {
        BusSystem.Register(Topics.Game.END_GAME, OnEndGame);
    }

    void InitButtons()
    {
        previousButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        complexAnimateButton.onClick.RemoveAllListeners();
        toggleShader.onClick.RemoveAllListeners();
        startGame.onClick.RemoveAllListeners();
        
        previousButton.onClick.AddListener(ChangeToPreviousShape);
        nextButton.onClick.AddListener(ChangeToNextShape);
        complexAnimateButton.onClick.AddListener(ComplexAnimate);
        toggleShader.onClick.AddListener(ToggleShader);
        startGame.onClick.AddListener(StartGame);
    }

    private void Start()
    {
        InitButtons();
    }


    public void ChangeToPreviousShape()
    {
        BusSystem.Execute(Topics.Base.CHANGE_TO_PREVIOUS_SHAPE);
        BusSystem.Execute(Topics.Audio.PLAY_SFX, new BusObject() {Content = new SfxInData() { SfxEnum = AudioEnums.SfxEnum.SFX_ButtonCLick, StopSfxWithSameName = true}});
    }
    
    public void ChangeToNextShape()
    {
        BusSystem.Execute(Topics.Base.CHANGE_TO_NEXT_SHAPE);
        BusSystem.Execute(Topics.Audio.PLAY_SFX, new BusObject() {Content = new SfxInData() { SfxEnum = AudioEnums.SfxEnum.SFX_ButtonCLick, StopSfxWithSameName = true}});
    }

    public void ComplexAnimate()
    {
        BusSystem.Execute(Topics.Base.COMPLEX_ANIMATE);
    }

    public void ToggleShader()
    {
        BusSystem.Execute(Topics.Base.TOGGLE_SHADER);
        BusSystem.Execute(Topics.Audio.PLAY_SFX, new BusObject() {Content = new SfxInData() { SfxEnum = AudioEnums.SfxEnum.SFX_ButtonCLick, StopSfxWithSameName = true}});
    }

    public void StartGame()
    {
        BusSystem.Execute(Topics.Game.START_GAME);
        BusSystem.Execute(Topics.Audio.PLAY_SFX, new BusObject() {Content = new SfxInData() { SfxEnum = AudioEnums.SfxEnum.SFX_ButtonCLick, StopSfxWithSameName = true}});
        
        Hide();
       
    }

    void OnEndGame(BusObject busObject)
    {
        Show();
    }

    
}
