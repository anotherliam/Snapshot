using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class GlobalGameState
    {

        private bool[] levelsCompleted = { false, false, false, false };

        public int SelectedLevelID = 1;
        public bool SoundEnabled = true;
        public bool MusicEnabled = true;

        public bool IsLevelCompleted(int levelID)
        {
            return levelsCompleted[levelID - 1];
        }
        public void SetLevelCompleted(int levelID, bool completed = true)
        {
            levelsCompleted[levelID - 1] = completed;
        }



        // Singletons are kinda an anti-pattern but google says this is the normal way of preserving state between scenes
        private static GlobalGameState state;
        public static GlobalGameState GameState
        {
            // Possible TODO: Look into saving settings?
            get
            {
                if (state == null)
                {
                    state = new GlobalGameState();
                }
                return state;
            }
        }

    }
}
