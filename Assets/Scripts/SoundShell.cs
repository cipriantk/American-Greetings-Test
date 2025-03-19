using UnityEngine;

[CreateAssetMenu(fileName = "SoundShell", menuName = "Scriptable Objects/SoundShell")]
public class SoundShell : ScriptableObject
{
    public AudioClip AudioCLip;
    public float Volume = 1;
    public Vector2 PitchRange = new Vector2(1f, 1f);
}
