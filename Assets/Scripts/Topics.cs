using JetBrains.Annotations;

public class Topics 
{
    public class Base
    {   
        public const string CHANE_OBJECT_COLOR = "ChangeObjectColor";
        public const string CHANGE_TO_NEXT_SHAPE = "ChangeToNextShape";
        public const string CHANGE_TO_PREVIOUS_SHAPE = "ChangeToPreviousShape";
        public const string COMPLEX_ANIMATE = "ComplexAnimate";
        public const string TOGGLE_SHADER = "ToggleShader";
    }
    
    public class Audio
    {   
        public const string PLAY_SFX = "PlaySFX";
        public const string STOP_SFX = "StopSFX";
    }

    public class Game
    {
        public const string START_GAME = "StartGame";
        public const string COLLIDED = "Collided";
        public const string END_GAME = "EndGame";
    }
    
    
}
