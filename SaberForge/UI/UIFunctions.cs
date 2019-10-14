using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using CustomUI.BeatSaber;
using CustomUI.UIElements;
using System;
using System.Collections.Generic;
using HMUI;
using System.Linq;

namespace SaberForge
{
    public class UIFunctions : MonoBehaviour
    {
        //to update part labels and hint text
        public class LabelTextGroup
        {
            public TextMeshProUGUI labelTMP;
            public HoverHint hoverHint;

            public PartEditor.PartList partList;
            public PartEditor.MatList matList;
           // public int listIndex;

            public LabelTextGroup(TextMeshProUGUI label, HoverHint hint, PartEditor.PartList list)
            {
                labelTMP = label;
                hoverHint = hint;
                partList = list;      
            }
            public LabelTextGroup(TextMeshProUGUI label, HoverHint hint, PartEditor.MatList list)
            {
                labelTMP = label;
                hoverHint = hint;
                matList = list;             
            }
        }

        public static List<LabelTextGroup> partTextGroups = new List<LabelTextGroup>();
        public static List<LabelTextGroup> matTextGroups = new List<LabelTextGroup>();

 
        public static void UpdatePartLabelText(LabelTextGroup textGroup)
        {

                PartInfo part = textGroup.partList.parts[textGroup.partList.index];
                textGroup.labelTMP.text = part.partDisplayName + " (" + (textGroup.partList.index + 1).ToString() + "/" + textGroup.partList.parts.Count.ToString() + ")";

                string[] partInfo = part.partReferenceName.Split('.');
                textGroup.hoverHint.text = "Author : " + partInfo[0] + "\nPart Bundle : " + partInfo[1];       
        }
        
        public static void UpdateMatLabelText(LabelTextGroup textGroup)
        {          
                //update material display name and hover text
                MaterialInfo matInfo = textGroup.matList.mats[textGroup.matList.index];

                textGroup.labelTMP.text = matInfo.materialDisplayName + " (" + (textGroup.matList.index + 1).ToString() + "/" + textGroup.matList.mats.Count.ToString() + ")";

                string[] matRefNameData = matInfo.materialReferenceName.Split('.');

                string ccText = "No";
                if (matInfo.supportsCustomColors)
                    ccText = "Yes";

                textGroup.hoverHint.text = "Author :  " + matRefNameData[0] + "\nPart Bundle : " + matRefNameData[1] + "\n\nUses Custom Colors : " + ccText;          
        }

        public static void UpdateAllPartLabels()
        {
            foreach (LabelTextGroup textGroup in partTextGroups)
            {
                UpdatePartLabelText(textGroup);
            }
        }
        public static void UpdateAllMatLabels()
        {
            foreach (LabelTextGroup textGroup in matTextGroups)
            {
                UpdateMatLabelText(textGroup);
            }
        }

        // creates some panels

        public static RectTransform CreateHorizontalLayoutObj(RectOffset padding, float spacing, TextAnchor alignment)
        {
            GameObject newLayoutObj = new GameObject("NewLayoutGroupObj", typeof(RectTransform));
            HorizontalLayoutGroup newLayout = newLayoutObj.AddComponent<HorizontalLayoutGroup>();
            newLayout.padding = padding;
            newLayout.spacing = spacing;
            newLayout.childAlignment = alignment;

            return newLayoutObj.GetComponent<RectTransform>();
        }

        public static RectTransform CreateVerticalLayoutObj(RectOffset padding, float spacing, TextAnchor alignment)
        {
            GameObject newLayoutObj = new GameObject("NewLayoutGroupObj", typeof(RectTransform));
            VerticalLayoutGroup newLayout = newLayoutObj.AddComponent<VerticalLayoutGroup>();
            newLayout.padding = padding;
            newLayout.spacing = spacing;
            newLayout.childAlignment = alignment;

            return newLayoutObj.GetComponent<RectTransform>();
        }

     
        public static GameObject CreateModelSwapPanel(Transform parent, PartEditor.PartList partList, string panelLabel)
        {
            RectTransform newPanel = CreateRectPanel(parent, panelLabel);

            TextMeshProUGUI partNameText = BeatSaberUI.CreateText(newPanel, "part name", new Vector2(20, 0), new Vector2(30, 10));
            partNameText.alignment = TextAlignmentOptions.Center;
            partNameText.fontSize = 4;

            HoverHint modelHoverHint = BeatSaberUI.AddHintText(partNameText.gameObject.GetComponent<RectTransform>(), "hint");

            LabelTextGroup textGroup = new LabelTextGroup(partNameText, modelHoverHint, partList);
            partTextGroups.Add(textGroup);

            //cry delegate won't let me pass int as a ref, hence faff with index arrays
            BeatSaberUI.CreateUIButton(newPanel, "DecButton", new Vector2(0, 0), new Vector2(8, 8), delegate { PartEditor.MoveThroughPartList(-1, partList); });
            BeatSaberUI.CreateUIButton(newPanel, "IncButton", new Vector2(50, 0), new Vector2(8, 8), delegate { PartEditor.MoveThroughPartList(1, partList); });

            //to set name and hint strings
            UpdatePartLabelText(textGroup);

            return newPanel.gameObject;
        }

        public static GameObject CreateMaterialSwapPanel(Transform parent, PartEditor.MatList matList, string panelLabel)
        {
            RectTransform newPanel = CreateRectPanel(parent, panelLabel);

            TextMeshProUGUI matNameText = BeatSaberUI.CreateText(newPanel, "mat name", new Vector2(20, 0), new Vector2(30, 10));
            matNameText.alignment = TextAlignmentOptions.Center;
            matNameText.fontSize = 4;

            HoverHint matHoverHint = BeatSaberUI.AddHintText(matNameText.gameObject.GetComponent<RectTransform>(), "hint");

            LabelTextGroup textGroup = new LabelTextGroup(matNameText, matHoverHint, matList);
            matTextGroups.Add(textGroup);

            //cry
            BeatSaberUI.CreateUIButton(newPanel, "DecButton", new Vector2(0, 0), new Vector2(8, 8), delegate { PartEditor.MoveThroughMatList(-1, matList); });
            BeatSaberUI.CreateUIButton(newPanel, "IncButton", new Vector2(50, 0), new Vector2(8, 8), delegate { PartEditor.MoveThroughMatList(1, matList); });

            //to set name and hint strings
            UpdateMatLabelText(textGroup);

            return newPanel.gameObject;
        }


        public static SFCustomSlider CreateSliderPanel(Transform parent, string panelLabel, PartEditor.PartAdjustment partAdjustment)
        {
            RectTransform newPanel = CreateRectPanel(parent, panelLabel);       

           // RectTransform sliderPanel = new GameObject("SliderPanel", typeof(RectTransform)).GetComponent<RectTransform>();
           // SetRect(sliderPanel, newPanel, new Vector2(0, 0), new Vector2(1, 1), new Vector2(21, 0), new Vector2(-50, 10));


            // Sliders currently broken in BSUI, yoinked code to implement fix
 

            SFCustomSlider slider = new GameObject("CustomUISlider").AddComponent<SFCustomSlider>();
            GameObject.DontDestroyOnLoad(slider.gameObject);
            slider.Scrollbar = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<HMUI.RangeValuesTextSlider>().First(s => s.name != "CustomUISlider"), parent, false);

            SetRect(slider.Scrollbar.GetComponent<RectTransform>(), newPanel, new Vector2(0, 0), new Vector2(1, 1), new Vector2(21, 0), new Vector2(-50, 10));

            slider.Scrollbar.name = "CustomUISlider";
            slider.Scrollbar.transform.SetParent(parent, false);
      
            slider.Scrollbar.numberOfSteps = (int)((partAdjustment.maxValue - partAdjustment.minValue) / partAdjustment.increment) + 1;
            slider.MinValue = slider.Scrollbar.minValue = partAdjustment.minValue;
            slider.MaxValue = slider.Scrollbar.maxValue = partAdjustment.maxValue;
            slider.IsIntValue = false;
            slider.Scrollbar.value = partAdjustment.value;

            UpdateSliderLabel(slider, partAdjustment.value);


            slider.Scrollbar.valueDidChangeEvent += delegate (RangeValuesTextSlider rangeValuesTextSlider, float value)
            {
                UpdateSliderLabel(slider, value);

                PartEditor.AdjustPartTransform(partAdjustment, value);

            };

            return slider;
        }

        public static void UpdateSliderLabel(CustomSlider slider, float value)
        {
            TextMeshProUGUI valueLabel = slider.Scrollbar.GetComponentInChildren<TextMeshProUGUI>();
            valueLabel.enableWordWrapping = false;
            //newSlider.SetCurrentValueFromPercentage(value);
            valueLabel.text = value.ToString("N2");

        }

        //BRUTE FORCE NO SURVIVORS
        public static void ForceSliderText(List<SFCustomSlider> sliderList)
        {
            foreach (SFCustomSlider slider in sliderList)
            {
                slider.StartCoroutine(slider.SetInitialText());
            }

        }

        public static void SetRect(RectTransform rect, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchorPos, Vector2 sizeDelta)
        {
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchorPos;
            rect.sizeDelta = sizeDelta;
        }

        public static RectTransform CreateRectPanel(Transform parent, String panelLabel)
        {
            RectTransform newPanel = new GameObject(panelLabel + "panel", typeof(RectTransform)).GetComponent<RectTransform>();
            SetRect(newPanel, parent, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 0), new Vector2(60, 30));

            TextMeshProUGUI labelText = BeatSaberUI.CreateText(newPanel, panelLabel, new Vector2(-35, 0), new Vector2(30, 10));
            labelText.alignment = TextAlignmentOptions.MidlineLeft;
            labelText.fontSize = 4;
            labelText.fontStyle = FontStyles.Bold;

            return newPanel;
        }

    }
}
