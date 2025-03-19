using System;
using System.Collections;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
   [SerializeField] private Delays delays;
   private void OnTriggerEnter2D(Collider2D other)
   {  
      BusSystem.Execute(Topics.Game.COLLIDED);
      StartCoroutine(EndGame());
   }

   IEnumerator EndGame()
   {  
      BusSystem.Execute(Topics.Audio.PLAY_SFX, new BusObject() {Content = new SfxInData() { SfxEnum = AudioEnums.SfxEnum.SFX_Game_Over, StopSfxWithSameName = true}});
      Time.timeScale = 0;
      yield return new WaitForSecondsRealtime(delays.GetLoseGameDelay());
      BusSystem.Execute(Topics.Game.END_GAME);
      Time.timeScale = 1;
   }
}
