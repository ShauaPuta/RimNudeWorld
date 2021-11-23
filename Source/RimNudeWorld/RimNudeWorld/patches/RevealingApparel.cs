using AlienRace;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;


namespace RevealingApparel
{



    //[StaticConstructorOnStartup]
    // public static class HarmonyPatching
    //    {

    //        static HarmonyPatching()
    //        {
    //            Harmony harmony = new Harmony("RevealingApparel");
    //            harmony.PatchAll();           
    //        }
    //    }





    public class ApparelRevealingExtension : DefModExtension
    {
        public List<RevealingExtensionEntry> revealingBodyPartEntries = new List<RevealingExtensionEntry>();

        public ApparelRevealingExtension()
        {

        }

    }

    public class RevealingExtensionEntry
    {

        public String revealingPath;
        public List<BodyTypeDef> revealingBodyTypes = new List<BodyTypeDef>();


    }





    public static class RevealingApparel
    {


        public static bool CanDrawRevealing(AlienPartGenerator.BodyAddon bodyAddon, Pawn pawn)
        {


            BodyTypeDef pawnBodyDef = pawn.story.bodyType;

            if (!(pawn.apparel.WornApparel == null) &&

                pawn.apparel.WornApparel.Where((Apparel ap)  //First fetching everything that covers the bodypart
                    => ap.def.apparel.bodyPartGroups.Any((BodyPartGroupDef bpgd)
                    => bodyAddon.hiddenUnderApparelFor.Contains(bpgd))
                    || ap.def.apparel.tags.Any((string s) => bodyAddon.hiddenUnderApparelTag.Contains(s)))


                    .All((Apparel ap) //Then checking that list, if everything has the revealing flag for the current body, reveal
                    => (ap.def.GetModExtension<ApparelRevealingExtension>()?.revealingBodyPartEntries.Any((RevealingExtensionEntry revealingExtensionEntry)
                    => (bodyAddon.path.Contains(revealingExtensionEntry.revealingPath) && revealingExtensionEntry.revealingBodyTypes.Any((BodyTypeDef revealingBodyType) => pawnBodyDef.defName.Contains(revealingBodyType.defName))))
                    ?? false

                    )))
            {

                Building_Bed building_Bed = pawn.CurrentBed();

                if ((building_Bed == null || building_Bed.def.building.bed_showSleeperBody || bodyAddon.drawnInBed) && (bodyAddon.backstoryRequirement.NullOrEmpty() || pawn.story.AllBackstories.Any((Backstory b) => b.identifier == bodyAddon.backstoryRequirement)))
                {
                    if (!bodyAddon.drawnDesiccated)
                    {
                        Corpse corpse = pawn.Corpse;
                        if (corpse != null && corpse.GetRotStage() == RotStage.Dessicated)
                        {
                            return false;

                        }
                    }
                    if (!bodyAddon.bodyPart.NullOrEmpty() && !pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord bpr) => bpr.untranslatedCustomLabel == bodyAddon.bodyPart || bpr.def.defName == bodyAddon.bodyPart))
                    {
                        List<AlienPartGenerator.BodyAddonHediffGraphic> list = bodyAddon.hediffGraphics;
                        bool flag;
                        if (list == null)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = list.Any((AlienPartGenerator.BodyAddonHediffGraphic bahg) => bahg.hediff == HediffDefOf.MissingBodyPart);
                        }
                        if (!flag)
                        {
                            return false;
                        }
                    }
                    if ((pawn.gender == Gender.Female) ? bodyAddon.drawForFemale : bodyAddon.drawForMale)
                    {
                        //return bodyAddon.bodyTypeRequirement.NullOrEmpty() || pawn.story.bodyType.ToString() == bodyAddon.bodyTypeRequirement;
                        return bodyAddon.bodyTypeRequirement.NullOrEmpty() || pawn.story.bodyType.ToString() == bodyAddon.bodyTypeRequirement;
                    }
                }
            }
            return false;
        }



    }

    [HarmonyPatch(typeof(AlienRace.HarmonyPatches), "DrawAddons")]
    class HarmonyPatch_DrawAddons
    {


        //[TweakValue("AAAInfrontAAAAAAAAtittyoffset", -0.1f, 0.1f)]
        private static float underClothingOffset = 0.0136f;
        //[TweakValue("AAABehindAAAAAAAAtittyoffset", -0.2f, 0.0f)]
        //private static float Behind = -0.1f;


        public static void Postfix(PawnRenderFlags renderFlags, Vector3 vector, Vector3 headOffset, Pawn pawn, Quaternion quat, Rot4 rotation)
        {

            ThingDef_AlienRace thingDef_AlienRace = pawn.def as ThingDef_AlienRace;
            if (thingDef_AlienRace == null || renderFlags.FlagSet(PawnRenderFlags.Invisible))
            {
                //Log.Message(pawn.def.defName);
                if (pawn.def.defName == "Human")
                {
                    //Log.Message(pawn.def.defName);
                }
                return;
            }





            Building_Bed building_Bed = pawn.CurrentBed();

            List<AlienPartGenerator.BodyAddon> bodyAddons = thingDef_AlienRace.alienRace.generalSettings.alienPartGenerator.bodyAddons;
            AlienPartGenerator.AlienComp comp = pawn.GetComp<AlienPartGenerator.AlienComp>();

            for (int i = 0; i < bodyAddons.Count; i++)
            {
                AlienPartGenerator.BodyAddon bodyAddon = bodyAddons[i];
                if (!bodyAddon.CanDrawAddon(pawn) && RevealingApparel.CanDrawRevealing(bodyAddon, pawn)) //No need to draw twice
                {


                    pawn.apparel.WornApparel.Any((Apparel ap)
                        => ap.def.apparel.bodyPartGroups.Any((BodyPartGroupDef bpgd)
                        => bodyAddon.hiddenUnderApparelFor.Contains(bpgd)));



                    AlienPartGenerator.RotationOffset offset = bodyAddon.defaultOffsets.GetOffset(rotation);
                    Vector3 a = (offset != null) ? offset.GetOffset(renderFlags.FlagSet(PawnRenderFlags.Portrait), pawn.story.bodyType, comp.crownType) : Vector3.zero;
                    AlienPartGenerator.RotationOffset offset2 = bodyAddon.offsets.GetOffset(rotation);
                    Vector3 vector2 = a + ((offset2 != null) ? offset2.GetOffset(renderFlags.FlagSet(PawnRenderFlags.Portrait), pawn.story.bodyType, comp.crownType) : Vector3.zero);


                    vector2.y = (bodyAddon.inFrontOfBody ? (0.3f + vector2.y - underClothingOffset) : (-0.3f - vector2.y + underClothingOffset));

                    // Log.Message(pawn.def.defName +"  "+ bodyAddon.path + " has vector2.y " + vector2.y + "Default offset: " + bodyAddon.defaultOffsets.GetOffset(rotation).layerOffset);

                    if (bodyAddon.inFrontOfBody && vector2.y < 0f) //The offset of some bodyaddons is too far out of the "over body, under clothes"-range, e.g. OTY bellies.
                    {
                        vector2.y = 0.01f;
                    }
                    // Log.Message(pawn.def.defName + "  " + bodyAddon.path + " has now vector2.y " + vector2.y + "Default offset: " + bodyAddon.defaultOffsets.GetOffset(rotation).layerOffset);

                    float num = bodyAddon.angle;
                    if (rotation == Rot4.North)
                    {
                        if (bodyAddon.layerInvert)
                        {
                            vector2.y = -vector2.y;
                            vector2.y -= underClothingOffset * 2; //I am not sure why I am doing this, but it puts Anus under the pants layer.
                        }
                        num = 0f;
                    }


                    if (rotation == Rot4.East)
                    {
                        num = -num;
                        vector2.x = -vector2.x;
                    }

                    Vector3 outputVector = vector + (bodyAddon.alignWithHead ? headOffset : Vector3.zero) + vector2.RotatedBy(Mathf.Acos(Quaternion.Dot(Quaternion.identity, quat)) * 2f * 57.29578f);


                    Graphic graphic = comp.addonGraphics[i];
                    graphic.drawSize = ((renderFlags.FlagSet(PawnRenderFlags.Portrait) && bodyAddon.drawSizePortrait != Vector2.zero) ? bodyAddon.drawSizePortrait : bodyAddon.drawSize) * (bodyAddon.scaleWithPawnDrawsize ? (bodyAddon.alignWithHead ? (renderFlags.FlagSet(PawnRenderFlags.Portrait) ? comp.customPortraitHeadDrawSize : comp.customHeadDrawSize) : (renderFlags.FlagSet(PawnRenderFlags.Portrait) ? comp.customPortraitDrawSize : comp.customDrawSize)) : Vector2.one) * 1.5f;
                    GenDraw.DrawMeshNowOrLater(
                        graphic.MeshAt(rotation),
                        outputVector,
                        Quaternion.AngleAxis(num, Vector3.up) * quat,
                        graphic.MatAt(rotation, null),
                        renderFlags.FlagSet(PawnRenderFlags.DrawNow));
                }


            }

            //return true;


        }




    }

}



