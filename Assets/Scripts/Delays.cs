using UnityEngine;

[CreateAssetMenu(fileName = "Delays", menuName = "Scriptable Objects/Delays")]
public class Delays : ScriptableObject
{   
    [SerializeField] private float asteroidBeginDelay;
    [SerializeField] private float asteroidSpawnDelay;
    [SerializeField] private float loseGameDelay;

    public float GetAsteroidBeginDelay()
    {
        return asteroidBeginDelay;
    }
    
    public float GetAsteroidSpawnDelay()
    {
        return asteroidSpawnDelay;
    }
    
    public float GetLoseGameDelay()
    {
        return loseGameDelay;
    }
    
}
