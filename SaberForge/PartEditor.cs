using UnityEngine;
using Unity;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace SaberForge
{
    public class PartEditor : MonoBehaviour
    {
        public class PartList
        {
            public string label;
            public List<PartInfo> parts;
            public int index;
            public string defaultPartRef;
            public PartList (string lab, List<PartInfo> partList, string dPartRef)
            {
                label = lab;
                parts = partList;
                defaultPartRef = dPartRef;
            }
        }
        public class MatList
        {
            public string label;
            public List<MaterialInfo> mats;
            public int index;
            public string defaultMatRef;
            public MatList(string lab, List<MaterialInfo> matList, string dMatRef)
            {
                label = lab;
                mats = matList;
                defaultMatRef = dMatRef;
            }
        }

        public static PartList BladeList = new PartList("Blade", new List<PartInfo>(), "FrostDragon.CoreParts.Blade.NoBlade");
        public static PartList GuardList = new PartList("Guard", new List<PartInfo>(), "FrostDragon.CoreParts.Guard.NoGuard");
        public static PartList HandleList = new PartList("Handle", new List<PartInfo>(), "FrostDragon.CoreParts.Handle.NoHandle");
        public static PartList PommelList = new PartList("Pommel", new List<PartInfo>(), "FrostDragon.CoreParts.Pommel.NoPommel");
        public static PartList AccAList = new PartList("AccessoryA", new List<PartInfo>(),"FrostDragon.CoreParts.AccessoryA.NoAcc");
        public static PartList AccBList = new PartList("AccessoryB", new List<PartInfo>(), "FrostDragon.CoreParts.AccessoryB.NoAcc");

        public static List<PartList> AllPartLists = new List<PartList>
        {
            BladeList, GuardList, HandleList, PommelList, AccAList, AccBList
        };

        public static MatList GlowList = new MatList("Glow", new List<MaterialInfo>(), "FrostDragon.CoreParts.Glow.Flat");
        public static MatList SecondaryList = new MatList("Secondary", new List<MaterialInfo>(), "FrostDragon.CoreParts.Secondary.Black");
        public static MatList TertiaryList = new MatList("Tertiary", new List<MaterialInfo>(), "FrostDragon.CoreParts.Secondary.Black");
        public static MatList NamePlateList = new MatList("NamePlate", new List<MaterialInfo>(), "FrostDragon.CoreParts.BlankNamePlate");
        public static MatList TrailList = new MatList("Trail", new List<MaterialInfo>(), "FrostDragon.CoreParts.Trail.Standard");
        public static MatList TemplateList = new MatList("Template", new List<MaterialInfo>(), ""); //for loading textures, not seen by player

        public static List<MatList> AllMatLists = new List<MatList>
        {
            GlowList, SecondaryList, TertiaryList, NamePlateList, TrailList,
        };

        public class PartAdjustment
        {
            public string type;
            public string label;
            public float value;
            public float minValue;
            public float maxValue;
            public float increment;
            public float defaultValue;

            public PartAdjustment (string t, string lab, float min, float max, float inc, float dVal)
            {
                type = t;
                label = lab;
                minValue = min;
                maxValue = max;
                increment = inc;
                value = defaultValue = dVal;
            }
        }

        public static PartAdjustment BladeXScale = new PartAdjustment("Adjustment", "BladeXScale", 0.05f, 1.5f, 0.05f, 1);
        public static PartAdjustment BladeYScale = new PartAdjustment("Adjustment", "BladeYScale", 0.05f, 1.5f, 0.05f, 1);
        public static PartAdjustment GuardScale = new PartAdjustment("Adjustment", "GuardScale", 0.5f, 2f, 0.05f, 1);
        public static PartAdjustment HandleScale = new PartAdjustment("Adjustment", "HandleScale", 0.5f, 1.5f, 0.05f, 1);
        public static PartAdjustment HandleLength = new PartAdjustment("Adjustment", "HandleLength", 0.5f, 1.5f, 0.05f, 1);
        public static PartAdjustment PommelScale = new PartAdjustment("Adjustment", "PommelScale", 0.5f, 2f, 0.05f, 1);

        public static PartAdjustment GuardAngle = new PartAdjustment("Adjustment", "GuardAngle", 0f, 360f, 10f, 0);

        public static PartAdjustment AccAPos = new PartAdjustment("AccessoryA", "AccessoryAPosition", -0.3f, 1.2f, 0.05f, 0.05f);
        public static PartAdjustment AccAScale = new PartAdjustment("AccessoryA", "AccessoryAScale", 0.05f, 1.5f, 0.05f, 1);
        public static PartAdjustment AccAAngle = new PartAdjustment("AccessoryA", "AccessoryAAngle", 0f, 360f, 10f, 0);
        public static PartAdjustment AccARotSpeed = new PartAdjustment("AccessoryA", "AccessoryARotationSpeed", 0f, 90f, 5f, 0);

        public static PartAdjustment AccBPos = new PartAdjustment("AccessoryB", "AccessoryBPosition", -0.3f, 1.2f, 0.05f, 0.05f);
        public static PartAdjustment AccBScale = new PartAdjustment("AccessoryB", "AccessoryBScale", 0.05f, 1.5f, 0.05f, 1);
        public static PartAdjustment AccBAngle = new PartAdjustment("AccessoryB", "AccessoryBAngle", 0f, 360f, 10f, 0);
        public static PartAdjustment AccBRotSpeed = new PartAdjustment("AccessoryB", "AccessoryBSRotationSpeed", 0f, 90f, 5f, 0);

        public static PartAdjustment TrailStart = new PartAdjustment("Trail", "TrailStart", 0.05f, 1, 0.05f, 1);
        public static PartAdjustment TrailEnd = new PartAdjustment("Trail", "TrailEnd", 0.05f, 1, 0.05f, 0.05f);

        public static List<PartAdjustment> AllPartAdjustments = new List<PartAdjustment>
        {
            BladeXScale, BladeYScale, GuardScale, HandleScale, HandleLength, PommelScale, GuardAngle,
            AccAPos, AccAScale, AccBAngle, AccARotSpeed, AccBPos, AccBScale, AccBAngle, AccBRotSpeed,
            TrailStart, TrailEnd
        };

        //public static float[] PartTransforms = new float[19]; //more stupid code

        //public enum TransIndex { BladeX, BladeY, GuardScale, HandleScale, HandleLength, PommelScale, GuardAngle,
        //    AccAScale, AccAPos, AccAAngle, AccARotSpeed,
        //    AccBScale, AccBPos, AccBAngle, AccBRotSpeed,
        //    TrailStartPos, TrailEndPos
        //};

        public static bool AccARotReverseLeft = false;
        public static bool AccARotReverseRight = false;

        public static bool AccBRotReverseLeft = false;
        public static bool AccBRotReverseRight = false;


        public delegate void OnPartModelChange();
        public static event OnPartModelChange onPartModelChange;

        public delegate void OnPartAdjusted();
        public static event OnPartAdjusted onPartAdjusted;

        public delegate void OnMaterialChange();
        public static event OnMaterialChange onMaterialChange;

        public delegate void OnTrailChange();
        public static event OnTrailChange onTrailChange;

        public static void AdjustPartTransform(PartAdjustment partAdjustment, float newValue)
        {
            partAdjustment.value = newValue;
            onPartAdjusted();
            onTrailChange();
        }

        public static void MoveThroughPartList(int step, PartList partList)
        {
            if (partList.parts.Count <= 0)
            {
                Logger.log.Debug("Part list is empty! Ensure mod part lists are installed in ../BeatSaber/SaberForgeParts");
                return;
            }

            partList.index += step;

            if (partList.index >= partList.parts.Count)
                partList.index = 0;

            else if (partList.index < 0)
                partList.index = partList.parts.Count - 1;

            //set text
            UIFunctions.UpdateAllPartLabels();

            if (step != 0)
                onPartModelChange();
        }

        public static void MoveThroughMatList(int step, MatList matList)
        {
            if (matList.mats.Count <= 0)
            {
                Logger.log.Debug("Material list is empty! Ensure mod part lists are installed in ../BeatSaber/SaberForgeParts");
                return;
            }

            matList.index += step;

            if (matList.index >= matList.mats.Count)
                matList.index = 0;

            else if (matList.index < 0)
                matList.index = matList.mats.Count - 1;

            //set text
            UIFunctions.UpdateAllMatLabels();

            if (step != 0)
                onMaterialChange();
        }

        public static Material FindMaterialInList(string matRefName, MatList matList)
        {
            Material mat = null;

            for (int i = 0; i < matList.mats.Count; i++)
            {
                if (matList.mats[i].materialReferenceName == matRefName)
                {
                    mat = matList.mats[i].material; 
                }

            }

            return mat;
        }

        public static void UpdateSabers()
        {
            onPartModelChange();
            onMaterialChange();
            onPartAdjusted();
            onTrailChange();
        }

    }
}
