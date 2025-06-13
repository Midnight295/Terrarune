using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;

namespace Terrarune.Core.ModPlayers.DrawLayers;
public interface IAnimatedHead
{
    Asset<Texture2D> headTexture { get; set; }

    int AnimationLength { get; }
    int AnimationDelay { get; }

    Color? CustomDrawColor => null;

    void Animate(Player player, ref int frameNum) {
        if (player.miscCounter % AnimationDelay == 0)
        {
            frameNum++;
            if (frameNum >= AnimationLength)
                frameNum = 0;
        }
    }
    bool PreDraw(PlayerDrawSet drawInfo) => true;
}
