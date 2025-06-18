using Terraria;
using Terraria.ModLoader;
using Terrarune.Core.ModPlayers.DrawLayers;

namespace Terrarune.Core.ModPlayers;
public class HeadAnimationPlayer : ModPlayer
{
    public int animationFrameNum = 0;

    public override void FrameEffects()
    {
        if (Player.head == -1)
        {
            animationFrameNum = 0;
            return;
        }

        if (EquipLoader.GetEquipTexture(EquipType.Head, Player.head) == null)
        {
            animationFrameNum = 0;
            return;
        }

        ModItem headItem = EquipLoader.GetEquipTexture(EquipType.Head, Player.head).Item;

        if (headItem is IAnimatedHead animated)
        {
            if (animated.Animate(Player, ref animationFrameNum))
            {
                if (Player.miscCounter % animated.AnimationDelay == 0)
                {
                    animationFrameNum++;
                    if (animationFrameNum >= animated.AnimationLength)
                        animationFrameNum = 0;
                }
            }
        }
        else
            animationFrameNum = 0;
    }
}
