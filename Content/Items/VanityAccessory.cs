using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terrarune.Content.Items
{
    public abstract class VanityAccessory : ModItem
    {
        public virtual string AssetPath => "Terrarune/Assets/Items/Vanity/";

        public virtual (EquipType Type, string AssetName, string EquipName)[] EquipSlots =>
        [
            (EquipType.Head, Item.ModItem.Name + "/" + Item.ModItem.Name, null),
            (EquipType.Body, Item.ModItem.Name + "/" + Item.ModItem.Name, null),
            (EquipType.Legs, Item.ModItem.Name + "/" + Item.ModItem.Name, null),
        ];

        public virtual bool ShouldHideAccessories => false;

        public virtual (SoundStyle sound, int delay)? HurtSound(Player p) => null;

        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                foreach (var v in EquipSlots)
                {
                    if (v.AssetName == null)
                        continue;
                    EquipLoader.AddEquipTexture(Mod, AssetPath + v.AssetName + v.Type.ToString(), v.Type, this, v.EquipName);
                }
            }
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {   
            if (incomingItem.ModItem != null && incomingItem.ModItem is VanityAccessory && incomingItem.type != Type)
                return false;
            return true;
        }

        public override void SetStaticDefaults()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ArmorIDSets();
        }

        public virtual void ArmorIDSets()
        {
            if (EquipSlots.Any(s => s.Type == EquipType.Head))
            {
                string name = EquipSlots.First(s => s.Type == EquipType.Head).EquipName;
                int equipSlotHead = EquipLoader.GetEquipSlot(Mod, name ?? Name, EquipType.Head);
                ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
            }

            if (EquipSlots.Any(s => s.Type == EquipType.Body))
            {
                string name = EquipSlots.First(s => s.Type == EquipType.Body).EquipName;
                int equipSlotBody = EquipLoader.GetEquipSlot(Mod, name ?? Name, EquipType.Body);
                ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
                ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
            }

            if (EquipSlots.Any(s => s.Type == EquipType.Legs))
            {
                string name = EquipSlots.First(s => s.Type == EquipType.Legs).EquipName;
                int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, name ?? Name, EquipType.Legs);
                ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
            }
        }

        public virtual void ModifyDrawInfo(ref PlayerDrawSet drawInfo) { }

        public virtual bool CustomSetEquipType(Player player, EquipType type, Mod mod, string name) => false;

        public virtual void TransformFrameEffects(Player player) { }

        public virtual void TransformPostUpdate(Player player) { }

    }
}
