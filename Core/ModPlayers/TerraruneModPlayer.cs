using Terraria;
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
        public int SusieLaughCounter = 0;

        ModItem currentVanityAccessory;

        public override void Load()
        {
            On_Player.UpdateVisibleAccessory += UpdateVanity;
        }


        public override void PreUpdate()
        {
            if (SusieLaughCounter > 0)
                SusieLaughCounter--;
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
        }
    }
}
