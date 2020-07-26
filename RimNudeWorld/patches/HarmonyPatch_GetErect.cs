using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;
using AlienRace;
using rjw;
using System.Reflection;
using UnityEngine;

namespace RimNudeWorld
{

    [StaticConstructorOnStartup]
    static class HarmonyPatchAll {

        static HarmonyPatchAll() {

            Harmony har = new Harmony("RimNudeWorld");
            har.PatchAll(Assembly.GetExecutingAssembly());

        }

    }


    [HarmonyPatch(typeof(AlienPartGenerator.BodyAddon), "GetPath")]
    public class GenitalPatch {

        public static void Postfix(Pawn pawn, ref Graphic __result) {

            if(__result?.path == null) {

                if (NudeSettings.debugMode) {
                    Log.Message("Original graphic that's trying to be replaced doesn't exist!");
                }
            }
            else {

                string originalPath = __result.path;

                if (pawn.Dead) {

                    if (NudeSettings.debugMode) {
                        Log.Message("Attempting to remove corpse genitals...");
                    }

                    if (pawn.Corpse != null && pawn.Corpse.CurRotDrawMode == RotDrawMode.Dessicated && ((originalPath.Length > 8 && originalPath.Contains("Genitals")) || (originalPath.Length > 7 && originalPath.Contains("Breasts")))) {
                        __result = GraphicDatabase.Get<Graphic_Multi>("Genitals/FeaturelessCrotch", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                    }
                    return;

                }
                else if (NudeSettings.pubicHair && originalPath.Length >= 5 && originalPath.Contains("Pubes")) {

                    __result = GraphicDatabase.Get<Graphic_Multi>("Genitals/Pubes/Shaved", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                    return;

                }
                else if (originalPath.Length >= 9 && originalPath.Contains("penis")) {

                    string modifiedPath = originalPath.Insert(9, "Flaccid/") + "_flaccid";

                    if (pawn.RaceHasSexNeed()) {

                        if (xxx.need_sex(pawn) > xxx.SexNeed.Frustrated && !(pawn.jobs.curDriver is JobDriver_Sex) && ContentFinder<Texture2D>.Get(modifiedPath + "_north", false) != null) {

                            Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(modifiedPath, __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                            __result = newGraphic;
                            if (NudeSettings.debugMode)
                                Log.Message("Modifying path " + originalPath + " with " + modifiedPath);
                            return;
                        }
                        else {
                            if (NudeSettings.debugMode)
                                Log.Message("Pawn is either: horny, has jobdriver sex, or texture at " + modifiedPath + " does not exist");
                        }

                    }
                    else {
                        if (NudeSettings.debugMode)
                            Log.Message("Pawn race does not have sexneed");
                    }


                }
                else {
                    if (NudeSettings.debugMode)
                        Log.Message(originalPath + " does not contain string \"penis\" or is shorter than a length of 9");
                }

            }

        }

    }

}
