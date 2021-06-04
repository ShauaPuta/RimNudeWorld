using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using rjw;
using UnityEngine;
using RimWorld;

namespace RimNudeWorld
{

    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveAllGraphics")]
    class HarmonyPatch_SwapBodyGraphicForRJW
    {

        public static void Postfix(PawnGraphicSet __instance)
        {

            string originalPath = __instance.nakedGraphic.path;
            string modifiedPath = originalPath + "_Lewd";

            if (!CachedGraphics.LewdGraphics.ContainsKey(modifiedPath))
            {
                Pawn p = __instance.pawn;
                if (ContentFinder<Texture2D>.Get(modifiedPath + "_north", false) != null)
                {
                    GraphicData originalGraphicData = p.ageTracker.CurKindLifeStage.bodyGraphicData;
                    Graphic lewdGraphic = GraphicDatabase.Get(originalGraphicData.graphicClass, modifiedPath, (originalGraphicData.shaderType == null ? ShaderTypeDefOf.Cutout.Shader : originalGraphicData.shaderType.Shader), originalGraphicData.drawSize, originalGraphicData.color, originalGraphicData.colorTwo, originalGraphicData, originalGraphicData.shaderParameters);

                    CachedGraphics.LewdGraphics.Add(modifiedPath, lewdGraphic);
                }

            }

        }

    }


}