using CustomUI.BeatSaber;
using CustomUI.UIElements;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using HMUI;

namespace SaberForge
{
    class RightViewController : CustomViewController
    {

        private RectTransform pageOneLayout;
        private RectTransform pageTwoLayout;

        private Button pageSwitchButton;

        private List<SFCustomSlider> pageOneSliders =  new List<SFCustomSlider>();
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

            //Sliders!!!

            pageOneSliders.Add(UIFunctions.CreateSliderPanel(pageOneLayout, "BLADE WIDTH", PartEditor.BladeXScale));
            pageOneSliders.Add(UIFunctions.CreateSliderPanel(pageOneLayout, "BLADE HEIGHT", PartEditor.BladeYScale));
            pageOneSliders.Add(UIFunctions.CreateSliderPanel(pageOneLayout, "GUARD SIZE", PartEditor.GuardScale));
            pageOneSliders.Add(UIFunctions.CreateSliderPanel(pageOneLayout, "HANDLE SIZE", PartEditor.HandleScale));
            pageOneSliders.Add(UIFunctions.CreateSliderPanel(pageOneLayout, "HANDLE LENGTH", PartEditor.HandleLength));
            pageOneSliders.Add(UIFunctions.CreateSliderPanel(pageOneLayout, "POMMEL SIZE", PartEditor.PommelScale));

            //PAGE TWO

            // add some buttons
            pageTwoLayout = UIFunctions.CreateVerticalLayoutObj(new RectOffset(6, 6, 6, 6), 1, TextAnchor.UpperCenter);
            UIFunctions.SetRect(pageTwoLayout, transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(60, -40), new Vector2(120, 70));

            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY A POSITION", PartEditor.AccAPos));
            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY A SIZE", PartEditor.AccAScale));

            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY B POSITION", PartEditor.AccBPos));
            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "ACCESSORY B SIZE", PartEditor.AccBScale));

            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "TRAIL START", PartEditor.TrailStart));
            pageTwoSliders.Add(UIFunctions.CreateSliderPanel(pageTwoLayout, "TRAIL END", PartEditor.TrailEnd));

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
            UIFunctions.ForceSliderText(pageOneSliders);
            UIFunctions.ForceSliderText(pageTwoSliders);
        }

    }
}