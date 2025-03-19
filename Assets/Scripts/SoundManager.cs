using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;



[System.Serializable]
public class SfxInData
{
    public AudioEnums.SfxEnum SfxEnum;
    public bool StopSfxWithSameName = false;
    public bool loop = false;
}

[System.Serializable]
public class SfxOutData
{
    public AudioEnums.SfxEnum SfxEnum;
    public AudioSource AudioSource;
}

public class SoundManager : MonoBehaviour
{  
    [SerializeField] private GameObject sfxAudioSource;
    [Header("SFX")] [SerializeField] private List<SoundShell> sfx;
    
    private List<SfxOutData> _sfxOutData = new List<SfxOutData>();
    private List<AudioSource> _audioSources = new List<AudioSource>();
    private void Awake()
    {
        BusSystem.Register(Topics.Audio.PLAY_SFX, PlaySFX);
        BusSystem.Register(Topics.Audio.STOP_SFX, StopSFX);
    }
  
    
    public void PlaySFX(BusObject obj)
    {
        SfxInData se = (SfxInData)obj.Content;

        if (se.StopSfxWithSameName == true)
        {
            for (int i = 0; i < _sfxOutData.Count; i++)
            {
                if (_sfxOutData[i].SfxEnum == se.SfxEnum)
                {
                    GameObject gmo = _sfxOutData[i].AudioSource.gameObject;
                    _sfxOutData[i].AudioSource.Stop();
                    _sfxOutData.Remove(_sfxOutData[i]);
                    gmo.SetActive(false);
                    //Destroy(gmo);
                }
            }
        }


        void SetupAudioSource(AudioSource source, SoundShell sfx)
        {
            source.playOnAwake = false;
            source.loop = se.loop;
                    
            source.volume = sfx.Volume;
            source.clip = sfx.AudioCLip;
            source.pitch = Random.Range(sfx.PitchRange.x, sfx.PitchRange.y);
            source.Play();
        }
        
        for (int i = 0; i < sfx.Count; i++)
        {
            if (sfx[i].name == se.SfxEnum.ToString())
            {

                AudioSource pooledAudioSource = FindUsedAudiosource();

                if (pooledAudioSource != null)
                {   
                    pooledAudioSource.gameObject.SetActive(true);
                    pooledAudioSource.gameObject.name = se.SfxEnum.ToString();

                    SetupAudioSource(pooledAudioSource, sfx[i]);
                    
                    SfxOutData sfxOutData = new SfxOutData() { SfxEnum = se.SfxEnum, AudioSource = pooledAudioSource };
                    _sfxOutData.Add(sfxOutData);
                }
                else
                {
                    GameObject n = new GameObject(se.SfxEnum.ToString());
                    n.transform.parent = sfxAudioSource.transform;
                    n.transform.localPosition = Vector3.zero;

                    AudioSource audioSource = n.AddComponent<AudioSource>();
                    SetupAudioSource(audioSource, sfx[i]);
                    
                    SfxOutData sfxOutData = new SfxOutData() { SfxEnum = se.SfxEnum, AudioSource = audioSource };
                    _sfxOutData.Add(sfxOutData);
                    _audioSources.Add(audioSource);
                }

               
                return;
            }
        }
    }
    
    
    
    
    public void StopSFX(BusObject obj)
    {
        AudioEnums.SfxEnum se = (AudioEnums.SfxEnum)obj.Content;

        for (int i = 0; i < _sfxOutData.Count; i++)
        {
            if (_sfxOutData[i].SfxEnum == se)
            {   
                GameObject gmo = _sfxOutData[i].AudioSource.gameObject;
                _sfxOutData[i].AudioSource.Stop();
                _sfxOutData.Remove(_sfxOutData[i]);
                gmo.SetActive(false);
               
            }
        }
    }

    private void Update()
    {
        HandleInstancedSfx();
    }

    private void HandleInstancedSfx()
    {
        for (int i = 0; i < _sfxOutData.Count; i++)
        {
            if (_sfxOutData[i].AudioSource.isPlaying == false)
            {
                GameObject gmo = _sfxOutData[i].AudioSource.gameObject;
                _sfxOutData[i].AudioSource.Stop();
                _sfxOutData.Remove(_sfxOutData[i]);
                gmo.SetActive(false);
                
            }
        }
    }

    private AudioSource FindUsedAudiosource()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].gameObject.activeSelf == false)
            {
                return _audioSources[i];
            }
        }
        
        return null;
    }

   
    



    
}
