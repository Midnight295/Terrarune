using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Core.ModPlayers.DrawLayers;

namespace Terrarune.Content.Items.Accessories.Vanity
{
    public class SusieChalk : VanityAccessory, IAnimatedHead
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/SusieChalk/SusieChalk";

        public Asset<Texture2D> headTexture { get; set; }

        public int AnimationLength => 3;

        public int AnimationDelay => 15;

        public bool Animate(Player player, ref int frameNum)
        {
            if (player.Terrarune().SusieLaughCounter > 0)
            {
                if (player.miscCounter % AnimationDelay == 0)
                {
                    if (frameNum == 1)
                        frameNum = 0;
                    else
                    {
                        frameNum = 1;
                        int index = CombatText.NewText(new Rectangle((int)player.Center.X + (player.direction * 32), (int)player.Center.Y + 32, 2, 2), Color.White, "HA", true);
                        Main.combatText[index].lifeTime = 15;
                    }
                }
            }
            else
                frameNum = 2;
            
            return false;
        }

        public override void Load()
        {
            base.Load();

            headTexture = ModContent.Request<Texture2D>(Texture + "Head");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.vanity = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return incomingItem.type != ModContent.ItemType<FluffyHat>() &&
                incomingItem.type != ModContent.ItemType<KrisKnife>() &&
                incomingItem.type != ModContent.ItemType<HornedHeadband>();
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().SusieChalk = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().SusieChalk = true;
        }
    }
}
