using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RimWorld;
using HarmonyLib;
using Verse;
using UnityEngine;
using AlienRace;
using rjw;

namespace RimNudeWorld
{

    [StaticConstructorOnStartup]
    public class BodyTypeAddon
    {

        public static bool IsLactating(Pawn pawn)
        {
            if (pawn != null)
            {
                if (pawn.RaceProps.Humanlike)
                {
                    foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                    {
                        if (hediff != null)
                        {
                            if (hediff.def.defName.Contains("Lactating") || hediff.def.defName.Contains("lactating"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //[HarmonyAfter(new string[] { "net.example.plugin2" })]
        [HarmonyPatch(typeof(AlienPartGenerator.BodyAddon), "GetPath")]
        class HARPatch
        {


            public static void Postfix(Pawn pawn, ref Graphic __result)
            {
                if (__result != null)
                {
                    if (pawn == null)
                        return;
                    string originalPath = __result.path;
                    bool validTexture = false;

                    //Body typed texture
                    if (pawn.story.bodyType == BodyTypeDefOf.Hulk || pawn.story.bodyType == BodyTypeDefOf.Fat)
                    {
                        if (pawn.story.bodyType == BodyTypeDefOf.Hulk)
                        {
                            if ((ContentFinder<Texture2D>.Get(originalPath + "_Hulk" + "_south", false) != null))
                            {
                                Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_Hulk", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                                __result = newGraphic;
                                validTexture = true;
                            }
                        }
                        else if (pawn.story.bodyType == BodyTypeDefOf.Fat)
                        {
                            if ((ContentFinder<Texture2D>.Get(originalPath + "_Fat" + "_south", false) != null))
                            {
                                Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_Fat", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                                __result = newGraphic;
                                validTexture = true;
                            }
                        }
                        if (validTexture == false)
                        {
                            if ((ContentFinder<Texture2D>.Get(originalPath + "_Wide" + "_south", false) != null))
                            {
                                Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_Wide", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                                __result = newGraphic;
                                validTexture = true;
                            }
                        }
                    }
                    else if (pawn.story.bodyType == BodyTypeDefOf.Thin)
                    {
                        if ((ContentFinder<Texture2D>.Get(originalPath + "_Thin" + "_south", false) != null))
                        {
                            Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_Thin", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                            __result = newGraphic;
                            validTexture = true;
                        }
                    }
                    else if (pawn.story.bodyType == BodyTypeDefOf.Male)
                    {
                        if ((ContentFinder<Texture2D>.Get(originalPath + "_Male" + "_south", false) != null))
                        {
                            Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_Male", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                            __result = newGraphic;
                            validTexture = true;
                        }
                    }
                    else if (pawn.story.bodyType == BodyTypeDefOf.Female)
                    {
                        if ((ContentFinder<Texture2D>.Get(originalPath + "_Female" + "_south", false) != null))
                        {
                            Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_Female", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                            __result = newGraphic;
                            validTexture = true;
                        }
                    }
                    else
                    {
                        string bodyname = pawn.story.bodyType.defName;
                        if ((ContentFinder<Texture2D>.Get(originalPath + "_" + bodyname + "_south", false) != null))
                        {
                            Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_" + bodyname, __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                            __result = newGraphic;
                            validTexture = true;
                        }
                    }

                    //lactation
                    if (IsLactating(pawn))
                    {
                        //Log.Message("finding Lactation Texture");
                        if ((ContentFinder<Texture2D>.Get(__result.path + "_Lactating" + "_south", false) != null))
                        {
                            Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(__result.path + "_Lactating", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                            __result = newGraphic;

                        }
                    }
                }


            }
        }

    }


}
