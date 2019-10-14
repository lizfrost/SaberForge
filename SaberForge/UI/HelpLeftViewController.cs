using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomUI.BeatSaber;
using UnityEngine;
using TMPro;



namespace SaberForge
{
    class HelpLeftViewController : CustomViewController
    {
      
            protected override void DidActivate(bool firstActivation, ActivationType type)
            {
                base.DidActivate(firstActivation, type);

                if (firstActivation) FirstActivation();
            }

            private void FirstActivation()
            {
                string tipsText = "<size=140%><b>Tips</b><size=110%>\n" +
                "\n" +
                "You can load your own trail and name textures into SaberForge! \n " +
                "Place .png files in /BeatSaber/SaberForgeAssets/UserTextures and follow the naming guide found there. \n" +
                "\n" +
              //  "For a live update of the sabers in your hand try installing the Custom Pointers mod by Kylon99.\n" +
               // "\n" +
                "For other saber tweaks, such as saber position and trail length, use the Saber Tailor mod.\n"

                ;


                RectTransform verticalLayout = UIFunctions.CreateVerticalLayoutObj(new RectOffset(6, 6, 6, 6), 1, TextAnchor.UpperCenter);
                UIFunctions.SetRect(verticalLayout,  transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(60, -40), new Vector2(120, 80));


                TextMeshProUGUI tips = BeatSaberUI.CreateText(verticalLayout, tipsText, new Vector2(0, 0), new Vector2(120, 80));

                //format
                tips.alignment = TextAlignmentOptions.Top;
                tips.enableWordWrapping = true;

            }
        
    }
}
