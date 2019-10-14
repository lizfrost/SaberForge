using CustomUI.BeatSaber;
using CustomUI.UIElements;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using HMUI;
using System.Collections.Generic;

namespace SaberForge
{
    class LeftViewController : CustomViewController
    {

        private RectTransform pageOneLayout;
        private RectTransform pageTwoLayout;

        private Button pageSwitchButton;

        private List<SFCustomSlider> pageTwoSliders = new List<SFCustomSlider>();


        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);

            if (firstActivation) FirstActivation();


        }

        private void FirstActivation()
        {
            // Page 1


            // add some buttons
            pageOneLayout = UIFunctions.CreateVerticalLayoutObj(new RectOffset(6, 6, 6, 6), 1, TextAnchor.UpperCenter);
            UIFunctions.SetRect(pageOneLayout, transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(60, -40), new Vector2(120, 70));

            pageSwitchButton = BeatSaberUI.CreateUIButton(transform.GetComponent<RectTransform>(), "PlayButton", new Vector2(48, 33), new Vector2(16, 16), delegate () { SwitchPages(); }, "1/2");

            UIFunctions.CreateMaterialSwapPanel(pageOneLayout, PartEditor.GlowList, "GLOW MATERIAL");
            UIFunctions.CreateMaterialSwapPanel(pageOneLayout, PartEditor.SecondaryList, "DETAIL MATERIAL");
            UIFunctions.CreateMaterialSwapPanel(pageOneLayout, PartEditor.TertiaryList, "HANDLE MATERIAL");
            UIFunctions.CreateMaterialSwapPanel(pageOneLayout, PartEditor.NamePlateList, "NAME PLATE MATERIAL");
            UIFunctions.CreateMaterialSwapPanel(pageOneLayout, PartEditor.TrailList, "TRAIL MATERIAL");

            //PAGE TWO



            // add some buttons
            pageTwoLayout = UIFunctions.CreateVerticalLayoutObj(new RectOffset(6, 6, 6, 6), 1, TextAnchor.UpperCenter);
            UIFunctions.SetRect(pageTwoLayout, transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(60, -40), new Vector2(120, 70));

            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "GUARD ANGLE", PartEditor.GuardAngle));

            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY A ANGLE", PartEditor.AccAAngle));
            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY A ROTATION", PartEditor.AccARotSpeed));

            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY B ANGLE", PartEditor.AccBAngle));
            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY B ROTATION", PartEditor.AccBRotSpeed));


          //  RectTransform accASpinPanel = UIFunctions.CreateRectPanel(pageTwoLayout, "ACCESSORY SPIN REVERSE");          
          //  BeatSaberUI.CreateUIButton(accASpinPanel, "PlayButton", new Vector2(0, 0), new Vector2(8, 8), delegate { PartEditor.AccARotReverseLeft = !PartEditor.AccARotReverseLeft; PartEditor.AccBRotReverseLeft = !PartEditor.AccBRotReverseLeft; }, "LEFT");
           // BeatSaberUI.CreateUIButton(accASpinPanel, "PlayButton", new Vector2(25, 0), new Vector2(8, 8), delegate { PartEditor.AccARotReverseRight = !PartEditor.AccARotReverseRight; PartEditor.AccBRotReverseRight = !PartEditor.AccBRotReverseRight; }, "RIGHT");


            pageTwoLayout.gameObject.SetActive(false);

        }

        private void SwitchPages()
        {

            pageOneLayout.gameObject.SetActive(!pageOneLayout.gameObject.activeSelf);

            pageTwoLayout.gameObject.SetActive(!pageTwoLayout.gameObject.activeSelf);

            if (pageOneLayout.gameObject.activeSelf)
                pageSwitchButton.SetButtonText("1/2");
            else
                pageSwitchButton.SetButtonText("2/2");

            ForceSliders();

        }
        public void ForceSliders()
        {

            UIFunctions.ForceSliderText(pageTwoSliders);

        }

    }
}
