using System;

namespace DefaultNamespace
{
    public class GameEvents
    {
        public static Action RestartLevel;
        public static Action StartLevel;
        public static Action<int> ScoreChanged;     // int = new score
        public static Action<int> LivesChanged; 
        public static Action GameOver;
        public static Action GameStarted;
        public static Action Intro;
    }
}