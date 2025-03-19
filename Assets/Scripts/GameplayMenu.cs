using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayMenu : Menu
{
   [SerializeField] private TextMeshProUGUI scoreText;
   [SerializeField] private GameObject gameInstructions;
   [Space] [SerializeField] private Delays delays;
   private float _score;
   private bool _canIncreaseScore = true;
   private void Awake()
   {
      BusSystem.Register(Topics.Game.START_GAME, OnStartGame);
      BusSystem.Register(Topics.Game.END_GAME, OnEndGame);
      BusSystem.Register(Topics.Game.COLLIDED, OnCollided);
   }

   private void OnStartGame(BusObject busObject)
   {
      Show();
      _score = 0;
      _canIncreaseScore = true;
      StartCoroutine(ShowInstructions());
   }
   
   private void OnEndGame(BusObject busObject)
   {
      Hide();
   }
   
   private void OnCollided(BusObject busObject)
   {
      _canIncreaseScore = false;
   }

   private void Update()
   {
      if (_canIncreaseScore == true)
      {
         _score += Time.deltaTime;
      }

      scoreText.text = "Score: " + (int)_score;
   }
   
   IEnumerator ShowInstructions()
   {
      gameInstructions.SetActive(true);
      yield return new WaitForSeconds(delays.GetAsteroidBeginDelay());
      gameInstructions.SetActive(false);
    
   }
}
