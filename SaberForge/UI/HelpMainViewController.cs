using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomUI.BeatSaber;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;



namespace SaberForge
{
    class HelpMainViewController : CustomViewController
    {

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);

            if (firstActivation) FirstActivation();
        }

        private void FirstActivation()
        {
            string helpText = "<size=150%><b>Instructions</b> \n" +
            "\n" +
            "<size=120%>1 - Adjust your sabers using the options provided in Saber Forge.\n" +
            "\n" +
            "2 - Open the Custom Sabers mod menu and choose the 'Saber Forge' sabers. \n" +
            "\n" +
            "3 - Hit bloq" 
            ;

            RectTransform verticalLayout = UIFunctions.CreateVerticalLayoutObj(new RectOffset(6, 6, 6, 6), 1, TextAnchor.UpperCenter);
            UIFunctions.SetRect(verticalLayout, transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(85, -40), new Vector2(150, 80));

            TextMeshProUGUI matLabelText = BeatSaberUI.CreateText(verticalLayout, helpText, new Vector2(0, 0), new Vector2(100, 80));

            //format
            matLabelText.alignment = TextAlignmentOptions.Top;

            //donate button
            Button donate = BeatSaberUI.CreateUIButton(transform as RectTransform, "PlayButton", new Vector2(-50, -33), new Vector2(50, 16), null, "DONATE <3");
            donate.onClick.AddListener(() => { GoTo("https://ko-fi.com/frostdragonliz", donate); });
            BeatSaberUI.AddHintText(donate.transform as RectTransform, "Kofi $$ will go towards future mod and part development, ta!");


            //reload assets button TO DO
          //  Button reload = BeatSaberUI.CreateUIButton(transform as RectTransform, "PlayButton", new Vector2(0, -33), new Vector2(50, 16), null, "USER TEXTURES");
          //  reload.onClick.AddListener(() => { Plugin.ReloadAssets(); });
          //  BeatSaberUI.AddHintText(reload.transform as RectTransform, "Force a reload of SaberForge Assets");

            //tutes button
            Button tutorials = BeatSaberUI.CreateUIButton(transform as RectTransform, "PlayButton", new Vector2(8, -33), new Vector2(50, 16), null, "TUTORIALS");
            tutorials.onClick.AddListener(() => { GoTo("https://www.youtube.com/playlist?list=PLyJh_4G6B76PPdL-rAmb44-475D0_0UBs", tutorials); });
            BeatSaberUI.AddHintText(tutorials.transform as RectTransform, "YouTube Tutorials");


            //github button
            Button github = BeatSaberUI.CreateUIButton(transform as RectTransform, "PlayButton", new Vector2(60, -33), new Vector2(50, 16), null, "SOURCE");
            github.onClick.AddListener(() => { GoTo("https://github.com/lizfrost/SaberForge", github); });
            BeatSaberUI.AddHintText(github.transform as RectTransform, "Source and mod info");



        }


        //Counters plus yoinked :3
        private void GoTo(string url, Button button)
        {

            button.interactable = false;
            TextMeshProUGUI reminder = BeatSaberUI.CreateText(transform as RectTransform, "Link opened in your browser!", new Vector2(0, -25), new Vector2(16, 16));
            reminder.fontSize = 4;
            reminder.alignment = TextAlignmentOptions.Center;

            this.StartCoroutine(SecondRemove(reminder.gameObject, button));
            System.Diagnostics.Process.Start(url);
        }

        private IEnumerator SecondRemove(GameObject go, Button button)
        {
            yield return new WaitForSeconds(5);
            Destroy(go);
            button.interactable = true;
        }

    }
}
