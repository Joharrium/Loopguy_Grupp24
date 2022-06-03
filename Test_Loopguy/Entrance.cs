using Microsoft.Xna.Framework;

namespace Test_Loopguy
{
    public class Entrance
    {
        int id;
        int level1;
        int level2;
        Rectangle hitbox;
        Vector2 target;

        public Entrance(int id, int lvl1, int lvl2, Rectangle gate, Vector2 target)
        {
            this.id = id;
            level1 = lvl1;
            level2 = lvl2;
            hitbox = gate;
            this.target = target;
        }

        internal void CheckGate(Player player)
        {
            if(hitbox.Contains(player.centerPosition) && LevelManager.GetCurrentId() == level1 && LevelManager.loadStarted == false)
            {                
                Fadeout.LevelTransitionFade();
                LevelManager.StartLevelTransition(level2, player, target);               
            }
        }
    }
}