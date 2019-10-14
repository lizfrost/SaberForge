using CustomUI.MenuButton;
using CustomUI.BeatSaber;


namespace SaberForge
{
    internal static class SaberForgeUI
    {
        internal static CustomMenu SaberForgeMenu;

        internal static CustomMenu HelpMenu;

        public static MainViewController mainViewController;
        public static RightViewController rightViewController;
        public static LeftViewController leftViewController;

        internal static void CreateMenu()
        {
            if (SaberForgeMenu == null)
            {
                SaberForgeMenu = BeatSaberUI.CreateCustomMenu<CustomMenu>("Saber Forge");

                //create views
                mainViewController = BeatSaberUI.CreateViewController<MainViewController>();
                rightViewController = BeatSaberUI.CreateViewController<RightViewController>();
                leftViewController = BeatSaberUI.CreateViewController<LeftViewController>();

                //set back button
                mainViewController.backButtonPressed += delegate ()
                {
                    SaberForgeMenu.Dismiss();
                };

                //set view positions
                SaberForgeMenu.SetMainViewController(mainViewController, true);
                SaberForgeMenu.SetRightViewController(rightViewController, false);
                SaberForgeMenu.SetLeftViewController(leftViewController, false);
            }

            if (HelpMenu == null)
            {
                HelpMenu = BeatSaberUI.CreateCustomMenu<CustomMenu>("Saber Forge Help");

                HelpMainViewController helpMainViewController = BeatSaberUI.CreateViewController<HelpMainViewController>();
                HelpRightViewController helpRightViewController = BeatSaberUI.CreateViewController<HelpRightViewController>();
                HelpLeftViewController helpLeftViewController = BeatSaberUI.CreateViewController<HelpLeftViewController>();

                //set back button
                helpMainViewController.backButtonPressed += delegate ()
                {
                    HelpMenu.Dismiss(); rightViewController.ForceSliders(); leftViewController.ForceSliders(); ;
                };

                //set view positions
                HelpMenu.SetMainViewController(helpMainViewController, true);
                HelpMenu.SetRightViewController(helpRightViewController, false);
                HelpMenu.SetLeftViewController(helpLeftViewController, false);

            }

            MenuButtonUI.AddButton("Saber Forge", delegate () { SaberForgeMenu.Present(); rightViewController.ForceSliders(); leftViewController.ForceSliders(); });
        }
    }
}