using CustomUI.BeatSaber;
using UnityEngine;


namespace SaberForge
{
    class MainViewController : CustomViewController
    {
        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);

            if (firstActivation)
                FirstActivation();

            Plugin.editorController.OnOpenEditor();
        }

        private void FirstActivation()
        {
            // add some buttons
            RectTransform verticalLayout = UIFunctions.CreateVerticalLayoutObj(new RectOffset(6, 6, 6, 6), 1, TextAnchor.UpperCenter);
            UIFunctions.SetRect(verticalLayout, transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(85, -40), new Vector2(150, 80));

            UIFunctions.CreateModelSwapPanel(verticalLayout, PartEditor.BladeList, "BLADE MODEL");
            UIFunctions.CreateModelSwapPanel(verticalLayout, PartEditor.GuardList,  "GUARD MODEL");
            UIFunctions.CreateModelSwapPanel(verticalLayout, PartEditor.HandleList,  "HANDLE MODEL");
            UIFunctions.CreateModelSwapPanel(verticalLayout, PartEditor.PommelList,  "POMMEL MODEL"); ;

            UIFunctions.CreateModelSwapPanel(verticalLayout, PartEditor.AccAList, "ACCESSORY A MODEL");
            UIFunctions.CreateModelSwapPanel(verticalLayout, PartEditor.AccBList,  "ACCESSORY B MODEL");

            BeatSaberUI.CreateUIButton(transform.GetComponent<RectTransform>(), "PlayButton", new Vector2(69,33), new Vector2(16, 16), delegate () { SaberForgeUI.HelpMenu.Present(); }, "?");

        }


        protected override void DidDeactivate(DeactivationType type)
        {
            base.DidDeactivate(type);

           Plugin.editorController.OnCloseEditor();


        }

    }
}
