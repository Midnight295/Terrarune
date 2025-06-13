using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terrarune.Common;

namespace Terrarune.Core.ModPlayers.DrawLayers;
public class AnimatedHeadLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        if (drawInfo.drawPlayer.head == -1)
            return false;

        if (EquipLoader.GetEquipTexture(EquipType.Head, drawInfo.drawPlayer.head) == null)
            return false;

        ModItem headItem = EquipLoader.GetEquipTexture(EquipType.Head, drawInfo.drawPlayer.head).Item;

        if (headItem is not IAnimatedHead)
            return false;

        return true;
    }

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;

        ModItem headItem = EquipLoader.GetEquipTexture(EquipType.Head, drawInfo.drawPlayer.head).Item;

        if (headItem is IAnimatedHead animatedHead)
        {
            string equipSlotName = headItem.Name;
            int equipSlot = EquipLoader.GetEquipSlot(Mod, equipSlotName, EquipType.Head);

            if (animatedHead.PreDraw(drawInfo) && !drawInfo.drawPlayer.dead && equipSlot == drawPlayer.head)
            {
                int dyeShader = drawPlayer.dye?[0].dye ?? 0;

                Vector2 headDrawPosition = drawInfo.Position - Main.screenPosition;
                headDrawPosition += new Vector2((drawPlayer.width - drawPlayer.bodyFrame.Width) / 2f, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f);
                headDrawPosition = new Vector2((int)headDrawPosition.X, (int)headDrawPosition.Y);
                headDrawPosition += drawPlayer.headPosition + drawInfo.headVect;

                Texture2D headTexture = animatedHead.headTexture.Value;

                Rectangle frame = headTexture.Frame(animatedHead.AnimationLength, 20, drawPlayer.HeadAnimationPlayer().animationFrameNum, drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height);
                
                DrawData pieceDrawData = new(headTexture, headDrawPosition, frame, drawInfo.colorArmorHead, drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0)
                {
                    shader = dyeShader
                };

                drawInfo.DrawDataCache.Add(pieceDrawData);

            }
        }
    }
}
