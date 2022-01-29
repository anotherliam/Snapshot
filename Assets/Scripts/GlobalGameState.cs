using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class GlobalGameState
    {

        public int SelectedLevelID = 1;

        // Singletons are kinda an anti-pattern but google says this is the normal way of preserving state between scenes
        private static GlobalGameState state;
        public static GlobalGameState GameState
        {
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
