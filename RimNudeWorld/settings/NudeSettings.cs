using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rjw;
using Verse;
using RimWorld;
using UnityEngine;

namespace RimNudeWorld {
    public class NudeSettings : ModSettings {

        public static bool debugMode = false, pubicHair = true;

        public override void ExposeData() {
            Scribe_Values.Look(ref debugMode, "RimNudeWorldDebugMode", false);
            Scribe_Values.Look(ref pubicHair, "PubicHair", false);
            base.ExposeData();
        }

    }


    public class RimNudeWorld : Mod {

        public RimNudeWorld(ModContentPack content) : base(content) {
            GetSettings<NudeSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect) {

            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("Debug Mode", ref NudeSettings.debugMode);
            listingStandard.CheckboxLabeled("Enable Pubic Hair", ref NudeSettings.pubicHair);

            listingStandard.End();

            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() {
            return "RimNudeWorld Settings";
        }


    }
}
