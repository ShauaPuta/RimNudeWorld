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

            string originalPath = __result.path;

            if (pawn.Dead) {

                if (NudeSettings.debugMode) {
                    Log.Message("Checking genital contains for " + originalPath);
                }

                if (pawn.Corpse != null && pawn.Corpse.CurRotDrawMode == RotDrawMode.Dessicated && ((originalPath.Length > 8 && originalPath.Contains("Genitals")) || (originalPath.Length > 7 && originalPath.Contains("Breasts")))) {
                    __result = null;
                }
                return;

            }

            if(NudeSettings.debugMode) {
                Log.Message("checking if path needs replacing");
            }

            else if (originalPath.Length >= 9 && originalPath.Contains("penis")) {
                if (NudeSettings.debugMode) {
                    Log.Message("flag 1");
                }

                string modifiedPath = originalPath.Insert(9, "Flaccid/") + "_flaccid";

                if (NudeSettings.debugMode) {
                    Log.Message("flag 2");
                }


                if (pawn.RaceHasSexNeed()) {

                    if (NudeSettings.debugMode) {
                        Log.Message("flag 3");
                    }

                    if (xxx.need_sex(pawn) > xxx.SexNeed.Frustrated && !(pawn.jobs.curDriver is JobDriver_Sex) && ContentFinder<Texture2D>.Get(modifiedPath + "_north", false) != null) {

                        if (NudeSettings.debugMode) {
                            Log.Message("flag 4");
                        }

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


            } else {
                if (NudeSettings.debugMode)
                    Log.Message(originalPath + " does not contain string \"penis\" or is shorter than a length of 9");
            }

        }

    }

}
