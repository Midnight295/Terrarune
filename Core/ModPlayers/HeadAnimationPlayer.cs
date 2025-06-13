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
            animated.Animate(Player, ref animationFrameNum);
        else
            animationFrameNum = 0;
    }
}
