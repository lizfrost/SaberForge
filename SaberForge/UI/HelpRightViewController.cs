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
    class HelpRightViewController : CustomViewController
    {
      
            protected override void DidActivate(bool firstActivation, ActivationType type)
            {
                base.DidActivate(firstActivation, type);

                if (firstActivation) FirstActivation();
            }

            private void FirstActivation()
            {
                string creditsText = "<b><size=140%>Developed by Frost Dragon</b> <size=100%>\n " +
                "\n" +
                "This mod would not be possible without the assitance of - \n" +
                "\n" +
                "The <b>Beat Saber Modding Group Discord</b> and <b>British Beat Saber Discord</b>.\n" +
                "The developers of BSIPA, BS Utils, CustomUI, Custom Sabers.\n " +
                "\n" +
                "With special thanks to community members -  \n" +
                "\n" +
                "Wolven "
                ;


                RectTransform verticalLayout = UIFunctions.CreateVerticalLayoutObj(new RectOffset(6, 6, 6, 6), 1, TextAnchor.UpperCenter);
                UIFunctions.SetRect(verticalLayout,  transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(60, -40), new Vector2(120, 80));


                TextMeshProUGUI credits = BeatSaberUI.CreateText(verticalLayout, creditsText, new Vector2(0, 0), new Vector2(120, 80));

                //format
                credits.alignment = TextAlignmentOptions.Top;
                credits.enableWordWrapping = true;

            }
        
    }
}
