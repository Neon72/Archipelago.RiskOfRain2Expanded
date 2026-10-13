using System.Collections.Generic;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace Archipelago.RiskOfRain2
{
    public static class TrapItems
    {
        public static ItemDef BaneAfflictionItemDef;
        public static ItemDef RavenousCheeseItemDef;

        public static void Init()
        {
            CreateBaneAffliction();
            CreateRavenousCheese();
            HookTrapItems();
        }

        private static void CreateBaneAffliction()
        {
            BaneAfflictionItemDef = ScriptableObject.CreateInstance<ItemDef>();
            BaneAfflictionItemDef.name = "BaneAffliction";
            BaneAfflictionItemDef.nameToken = "ARCHIPELAGO_BANE_AFFLICTION_NAME";
            BaneAfflictionItemDef.pickupToken = "ARCHIPELAGO_BANE_AFFLICTION_PICKUP";
            BaneAfflictionItemDef.descriptionToken = "ARCHIPELAGO_BANE_AFFLICTION_DESC";
            BaneAfflictionItemDef.loreToken = "ARCHIPELAGO_BANE_AFFLICTION_LORE";
            BaneAfflictionItemDef.tier = ItemTier.NoTier; 
            BaneAfflictionItemDef.hidden = true;
            BaneAfflictionItemDef.canRemove = false;
            BaneAfflictionItemDef.tags = new ItemTag[] { ItemTag.WorldUnique, ItemTag.CannotCopy, ItemTag.CannotDuplicate };

            LanguageAPI.Add("ARCHIPELAGO_BANE_AFFLICTION_NAME", "Bane Affliction");
            LanguageAPI.Add("ARCHIPELAGO_BANE_AFFLICTION_PICKUP", "Permanently reduces stats.");
            LanguageAPI.Add("ARCHIPELAGO_BANE_AFFLICTION_DESC", "Reduces max health, damage, attack speed, movement speed, and regeneration by 5%.");

            ItemAPI.Add(new CustomItem(BaneAfflictionItemDef, new ItemDisplayRuleDict()));
        }

        private static void CreateRavenousCheese()
        {
            RavenousCheeseItemDef = ScriptableObject.CreateInstance<ItemDef>();
            RavenousCheeseItemDef.name = "RavenousCheese";
            RavenousCheeseItemDef.nameToken = "ARCHIPELAGO_RAVENOUS_CHEESE_NAME";
            RavenousCheeseItemDef.pickupToken = "ARCHIPELAGO_RAVENOUS_CHEESE_PICKUP";
            RavenousCheeseItemDef.descriptionToken = "ARCHIPELAGO_RAVENOUS_CHEESE_DESC";
            RavenousCheeseItemDef.loreToken = "ARCHIPELAGO_RAVENOUS_CHEESE_LORE";
            RavenousCheeseItemDef.tier = ItemTier.NoTier;
            RavenousCheeseItemDef.hidden = false;
            RavenousCheeseItemDef.canRemove = true;
            RavenousCheeseItemDef.tags = new ItemTag[] { ItemTag.WorldUnique, ItemTag.CannotCopy, ItemTag.CannotDuplicate };

            LanguageAPI.Add("ARCHIPELAGO_RAVENOUS_CHEESE_NAME", "Ravenous Cheese");
            LanguageAPI.Add("ARCHIPELAGO_RAVENOUS_CHEESE_PICKUP", "Consumes your items periodically, no refunds.");
            LanguageAPI.Add("ARCHIPELAGO_RAVENOUS_CHEESE_DESC", "Every minute, <color=#FF7F7F>consumes a random item</color> and converts it into another <color=#E5C158>Ravenous Cheese</color>.");
            
            ItemCatalog.availability.CallWhenAvailable(() =>
            {
                RavenousCheeseItemDef.pickupModelPrefab = DLC1Content.Items.LunarSun.pickupModelPrefab;
                RavenousCheeseItemDef.pickupIconSprite = DLC1Content.Items.LunarSun.pickupIconSprite;
            });

            ItemAPI.Add(new CustomItem(RavenousCheeseItemDef, new ItemDisplayRuleDict()));
        }

        private static void HookTrapItems()
        {
            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                if (sender.inventory)
                {
                    int baneCount = sender.inventory.GetItemCount(BaneAfflictionItemDef);
                    if (baneCount > 0)
                    {
                        float reduction = 0.05f * baneCount;
                        args.healthMultAdd -= reduction;
                        args.damageMultAdd -= reduction;
                        args.attackSpeedMultAdd -= reduction;
                        args.moveSpeedMultAdd -= reduction;
                        args.regenMultAdd -= reduction;
                    }
                }
            };

            RoR2Application.onFixedUpdate += FixedUpdateCheese;
        }

        private static float cheeseTimer = 0f;
        private static void FixedUpdateCheese()
        {
            if (NetworkServer.active)
            {
                cheeseTimer += Time.fixedDeltaTime;
                if (cheeseTimer >= 60f)
                {
                    cheeseTimer = 0f;
                    foreach (var master in CharacterMaster.readOnlyInstancesList)
                    {
                        if (master.inventory)
                        {
                            int cheeseCount = master.inventory.GetItemCount(RavenousCheeseItemDef);
                            if (cheeseCount > 0)
                            {
                                List<ItemIndex> consumableItems = new List<ItemIndex>();
                                foreach (var itemIndex in master.inventory.itemAcquisitionOrder)
                                {
                                    var itemDef = ItemCatalog.GetItemDef(itemIndex);
                                    if (itemDef != null && itemDef != RavenousCheeseItemDef && itemDef.tier != ItemTier.NoTier && itemDef.tier != ItemTier.AssignedAtRuntime)
                                    {
                                        consumableItems.Add(itemIndex);
                                    }
                                }

                                if (consumableItems.Count > 0)
                                {
                                    var itemToEat = consumableItems[UnityEngine.Random.Range(0, consumableItems.Count)];
                                    master.inventory.RemoveItem(itemToEat, 1);
                                    master.inventory.GiveItem(RavenousCheeseItemDef, 1);
                                    CharacterMasterNotificationQueue.SendTransformNotification(master, itemToEat, RavenousCheeseItemDef.itemIndex, CharacterMasterNotificationQueue.TransformationType.LunarSun);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
