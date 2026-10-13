using Archipelago.RiskOfRain2.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archipelago.RiskOfRain2.Lookup
{
    public class LocationNames
    {
        public static readonly Dictionary<int, string> locationsNames = new()
        {
            { 3, "Aphelian Sanctuary" },
            { 7, "Distant Roost" },
            { 8, "Distant Roost (2)" },
            { 28, "Verdant Falls"},
            { 10, "Abyssal Depths" },
            { 12, "Wetland Aspect" },
            { 13, "Rallypoint Delta" },
            { 15, "Titanic Plains" },
            { 16, "Titanic Plains (2)" },
            { 17, "Abandoned Aqueduct" },
            { 35, "Sundered Grove" },
            { 37, "Siren's Call" },
            { 38, "Sky Meadow" },
            { 39, "Siphoned Forest" },
            { 41, "Sulfur Pools" },
            { 47, "Scorched Acres" },
            { 32, "Commencement" },
            { 4, "Void Fields" },
            { 46, "Void Locus" },
            { 45, "The Planetarium" },
            { 5, "Hidden Realm: Bulwark's Ambry"},
            { 6, "Hidden Realm: Bazaar Between Time"},
            { 14, "Hidden Realm: Gilded Coast" },
            { 27, "Hidden Realm: A Moment, Whole"},
            { 33, "Hidden Realm: A Moment, Fractured" },
            { 34, "Viscous Falls" },
            { 54, "Shattered Abodes" },
            { 55, "Disturbed Impact" },
            { 36, "Reformed Altar" },
            { 21, "Treeborn Colony" },
            { 22, "Golden Dieback" },
            { 23, "Helminth Hatchery" },
            { 40, "Prime Meridian" },
            { 60, "Pretender's Precipice" },
            { 61, "Iron Alluvium" },
            { 62, "Iron Auroras" },
            { 63, "Repurposed Crater" },
            { 64, "Conduit Canyon" },
            { 65, "Solutional Haunt" },
            { 66, "Computational Exchange" },
            { 67, "Neural Sanctum" },
            { 70, "Damp Caverns" },
            { 71, "Tenebrous Quagmire" },
            { 72, "Lurid Basilica" },
            { 73, "Soulbound Threshold" },
            { 74, "Terminal Lucidity" }
        };

        public static readonly Dictionary<int, string> cachedLocationsNames = new()
        {
            { 3, "ancientloft" },
            { 4, "arena" },
            { 5, "artifactworld" },
            { 6, "bazaar" },
            { 7, "blackbeach" },
            { 8, "blackbeach2" },
            { 10, "dampcavesimple" },
            { 12, "foggyswamp" },
            { 13, "frozenwall" },
            { 14, "goldshores" },
            { 15, "golemplains" },
            { 16, "golemplains2" },
            { 17, "goolake" },
            { 27, "limbo" },
            { 28, "lakes"},
            { 32, "moon2" },
            { 33, "mysteryspace" },
            { 35, "rootjungle" },
            { 37, "shipgraveyard" },
            { 38, "skymeadow" },
            { 39, "snowyforest" },
            { 41, "sulfurpools" },
            { 45, "voidraid" },
            { 46, "voidstage" },
            { 47, "wispgraveyard" },
            { 34, "lakesnight" },
            { 54, "village" },
            { 55, "villagenight" },
            { 36, "lemuriantemple" },
            { 21, "habitat" },
            { 22, "habitatfall" },
            { 23, "helminthroost" },
            { 40, "meridian" },
            { 60, "nest" },
            { 61, "ironalluvium" },
            { 62, "ironalluvium2" },
            { 63, "repurposedcrater" },
            { 64, "conduitcanyon" },
            { 65, "solutionalhaunt" },
            { 66, "computationalexchange" },
            { 67, "solusweb" },
            { 70, "fungal" },
            { 71, "mire" },
            { 72, "sanctum" },
            { 73, "champrush" },
            { 74, "ultima" },
        };

        public static readonly Dictionary<int, string> artifactLocations = new()
        {
            { 43000, "Artifact: Chaos" },
            { 43001, "Artifact: Command" },
            { 43002, "Artifact: Death" },
            { 43003, "Artifact: Dissonance" },
            { 43004, "Artifact: Enigma" },
            { 43005, "Artifact: Evolution" },
            { 43006, "Artifact: Frailty" },
            { 43007, "Artifact: Glass" },
            { 43008, "Artifact: Honor" },
            { 43009, "Artifact: Kin" },
            { 43010, "Artifact: Metamorphosis" },
            { 43011, "Artifact: Sacrifice" },
            { 43012, "Artifact: Soul" },
            { 43013, "Artifact: Spite" },
            { 43014, "Artifact: Swarms" },
            { 43015, "Artifact: Vengeance" },
            { 43016, "Artifact: Delusion" },
            { 43017, "Artifact: Devotion" },
            { 43018, "Artifact: Prestige" }
        };

        public string GetLocationName(string cachedName)
        {
            int sceneIndex = GetSceneIndex(cachedName);
            if (locationsNames.TryGetValue(sceneIndex, out string locationName))
            {
                return locationName;
            }
            return "";
        }

        public string GetLocationNameByIndex(int index)
        {
            if (locationsNames.TryGetValue(index, out string locationName))
            {
                return locationName;
            }
            return "";
        }
        public string GetCachedLocationNameByIndex(int index)
        {
            if (cachedLocationsNames.TryGetValue(index, out string cachedName))
            {
                return cachedName;
            }
            return "";
        }

        public bool LocationNamesContains(string sceneName)
        {
            return locationsNames.ContainsValue(sceneName);
        }

        public bool CachedLocationNamesContains(string cachedName)
        {
            return cachedLocationsNames.ContainsValue(cachedName);
        }

        public int GetSceneIndex(string cachedName)
        {
            foreach (var scene in cachedLocationsNames)
            {
                if (scene.Value == cachedName)
                {
                    return scene.Key;
                }
            }
            return 0;
        }

    }
}
