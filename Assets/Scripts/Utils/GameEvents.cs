using System;
using Hidden_Points_System;

namespace DefaultNamespace
{
    public class GameEvents
    {
        public static Action RestartLevel;
        public static Action StartLevel;
        public static Action<int> ScoreChanged;    
        public static Action<int> LivesChanged; 
        public static Action GameOver;
        public static Action GameStarted;
        public static Action Intro;
        public static Action OnWaveEnded;
        public static Action<WaveConfig> OnWaveStarted;


    }
}