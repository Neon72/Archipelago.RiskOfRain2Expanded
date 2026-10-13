using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net;
using RoR2;
using UnityEngine;

namespace Archipelago.RiskOfRain2.Handlers
{
    class SurvivorLockHandler : IHandler
    {
        private ArchipelagoSession session;
        private bool shuffleSurvivors = false;

        private Dictionary<string, long> survivorToApItemId = new Dictionary<string, long>
        {
            { "Commando", 37800 },
            { "Huntress", 37801 },
            { "Bandit2", 37802 },
            { "Toolbot", 37803 },
            { "Engi", 37804 },
            { "Mage", 37805 },
            { "Merc", 37806 },
            { "Treebot", 37807 },
            { "Loader", 37808 },
            { "Croco", 37809 },
            { "Captain", 37810 },
            { "Railgunner", 37811 },
            { "VoidSurvivor", 37812 },
            { "Seeker", 37813 },
            { "Chef", 37814 },
            { "FalseSon", 37815 },
            { "Drifter", 37816 },
            { "Operator", 37817 },
            { "Enforcer", 37818 },
            { "Acolyte", 37819 }
        };

        public SurvivorLockHandler(ArchipelagoSession session, bool shuffleSurvivors)
        {
            this.session = session;
            this.shuffleSurvivors = shuffleSurvivors;
        }

        public void Hook()
        {
            if (shuffleSurvivors)
            {
                On.RoR2.UserProfile.HasSurvivorUnlocked += UserProfile_HasSurvivorUnlocked;
                On.RoR2.UserProfile.HasUnlockable_UnlockableDef += UserProfile_HasUnlockable_UnlockableDef;
                On.RoR2.UserProfile.HasUnlockable_string += UserProfile_HasUnlockable_string;
            }
        }

        public void UnHook()
        {
            if (shuffleSurvivors)
            {
                On.RoR2.UserProfile.HasSurvivorUnlocked -= UserProfile_HasSurvivorUnlocked;
                On.RoR2.UserProfile.HasUnlockable_UnlockableDef -= UserProfile_HasUnlockable_UnlockableDef;
                On.RoR2.UserProfile.HasUnlockable_string -= UserProfile_HasUnlockable_string;
            }
        }

        private bool IsSurvivorUnlocked(string cachedName)
        {
            if (survivorToApItemId.TryGetValue(cachedName, out long itemId))
            {
                bool unlocked = session.Items.AllItemsReceived.Any(item => item.ItemId == itemId);
                return unlocked;
            }

            // Dynamically assign modded survivors
            long nextModdedId = 37820 + survivorToApItemId.Count - 20;
            survivorToApItemId[cachedName] = nextModdedId;

            bool moddedUnlocked = session.Items.AllItemsReceived.Any(item => item.ItemId == nextModdedId);
            return moddedUnlocked;
        }

        private bool UserProfile_HasSurvivorUnlocked(On.RoR2.UserProfile.orig_HasSurvivorUnlocked orig, UserProfile self, SurvivorIndex survivorIndex)
        {
            SurvivorDef survivorDef = SurvivorCatalog.GetSurvivorDef(survivorIndex);
            if (survivorDef != null)
            {
                return IsSurvivorUnlocked(survivorDef.cachedName);
            }
            return orig(self, survivorIndex);
        }

        private bool UserProfile_HasUnlockable_UnlockableDef(On.RoR2.UserProfile.orig_HasUnlockable_UnlockableDef orig, UserProfile self, UnlockableDef unlockableDef)
        {
            if (unlockableDef != null)
            {
                foreach (var kvp in survivorToApItemId)
                {
                    SurvivorDef survivorDef = SurvivorCatalog.FindSurvivorDef(kvp.Key);
                    if (survivorDef != null && survivorDef.unlockableDef == unlockableDef)
                    {
                        return IsSurvivorUnlocked(kvp.Key);
                    }
                }
            }
            return orig(self, unlockableDef);
        }

        private bool UserProfile_HasUnlockable_string(On.RoR2.UserProfile.orig_HasUnlockable_string orig, UserProfile self, string unlockableToken)
        {
            if (!string.IsNullOrEmpty(unlockableToken))
            {
                foreach (var kvp in survivorToApItemId)
                {
                    SurvivorDef survivorDef = SurvivorCatalog.FindSurvivorDef(kvp.Key);
                    if (survivorDef != null && survivorDef.unlockableDef != null && survivorDef.unlockableDef.cachedName == unlockableToken)
                    {
                        return IsSurvivorUnlocked(kvp.Key);
                    }
                }
            }
            return orig(self, unlockableToken);
        }
    }
}
