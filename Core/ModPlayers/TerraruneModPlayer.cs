﻿using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Content.Items;

namespace Terrarune.Core.ModPlayers
{
    public class TerraruneModPlayer : ModPlayer
    {
        public bool KrisKnife;
        public bool SusieChalk;
        public bool FluffyHat;
        public bool HornedHeadband;
        public bool BerdlyGrade;
        public int SusieLaughCounter = 0;
        public int ItemUsedPreviousFrame = 0;
        //for usage in complex swings where you want to store an initial use rotation while animated player.itemRotation
        public float TargetRotation = 0f;
        //an extra variable to use for custom use styles
        public float UseStyleVar = 0f;
        SoundStyle SusieLaugh = new("Terrarune/Assets/Sounds/SusieLaugh");

        ModItem currentVanityAccessory;

        public override void Load()
        {
            On_Player.UpdateVisibleAccessory += UpdateVanity;
        }
       
        public override void PreUpdate()
        {
            if (SusieLaughCounter > 0)
            {
                if(SusieLaughCounter == 90)
                    SoundEngine.PlaySound(SusieLaugh, Player.Center);
                SusieLaughCounter--;
            }
            
        }
        public override void PostUpdate()
        {
            if (Player.ItemAnimationActive)
            {
                ItemUsedPreviousFrame = Player.HeldItem.type;
            }
            else
            {
                ItemUsedPreviousFrame = 0;
            }
        }

        public override void UpdateDead()
        {
            SusieLaughCounter = 0;
        }

        private void UpdateVanity(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
        {
            orig(self, itemSlot, item, modded);

            if (item.ModItem is VanityAccessory vanity)
            {
                self.legs = EquipLoader.GetEquipSlot(Mod, item.ModItem.Name, EquipType.Legs);
                self.body = EquipLoader.GetEquipSlot(Mod, item.ModItem.Name, EquipType.Body);
                self.head = EquipLoader.GetEquipSlot(Mod, item.ModItem.Name, EquipType.Head);
            }

            if (itemSlot == 18)
            {
                if (self.head == -1)
                {
                    self.Terrarune().currentVanityAccessory = null;
                    return;
                }

                if (EquipLoader.GetEquipTexture(EquipType.Head, self.head) == null)
                {
                    self.Terrarune().currentVanityAccessory = null;
                    return;
                }

                ModItem headItem = EquipLoader.GetEquipTexture(EquipType.Head, self.head).Item;

                if (headItem is VanityAccessory)
                    self.Terrarune().currentVanityAccessory = headItem;
                else
                    self.Terrarune().currentVanityAccessory = null;
            }
        }

        public override void UpdateAutopause()
        {
            if (currentVanityAccessory != null)
                Player.head = EquipLoader.GetEquipSlot(Mod, currentVanityAccessory.Name, EquipType.Head);
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (currentVanityAccessory != null)
                ((VanityAccessory)currentVanityAccessory).ModifyDrawInfo(ref drawInfo);
        }

        public override void ResetEffects()
        {
            KrisKnife = false;
            SusieChalk = false;
            FluffyHat = false;
            HornedHeadband = false;
            BerdlyGrade = false;
        }
        //attempt to make stuff work in multiplayer. did  work!!!.
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket pack = ModContent.GetInstance<Terrarune>().GetPacket();
            pack.Write7BitEncodedInt(0);
            pack.Write7BitEncodedInt(Player.whoAmI);
            pack.Write7BitEncodedInt(ItemUsedPreviousFrame);
            pack.Write(TargetRotation);
            pack.Write(UseStyleVar);
            pack.Send(toWho, fromWho);
        }
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            TerraruneModPlayer clone = (TerraruneModPlayer)clientPlayer;
            if (clone.TargetRotation != TargetRotation ||
                clone.ItemUsedPreviousFrame != ItemUsedPreviousFrame ||
                clone.UseStyleVar != UseStyleVar)
            {
                SyncPlayer(-1, Main.myPlayer, false);
            }
        }
        public override void CopyClientState(ModPlayer targetCopy)
        {
            TerraruneModPlayer clone = (TerraruneModPlayer)targetCopy;
            clone.TargetRotation = TargetRotation;
            clone.ItemUsedPreviousFrame = ItemUsedPreviousFrame;
            clone.UseStyleVar = UseStyleVar;
        }
    }
}
