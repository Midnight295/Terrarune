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

    bool Animate(Player player, ref int frameNum);
    bool PreDraw(PlayerDrawSet drawInfo) => true;
}
