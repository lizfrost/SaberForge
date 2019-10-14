using System.Collections.Generic;
using BS_Utils.Utilities;
using UnityEngine;


namespace SaberForge
{
    class SaberForgeConfig
    {
        //save and load saber settings

        public static Config SFConfig = new Config("SaberForgeConfig");
        public bool RegenerateConfig = true;

        public static void SaveConfig()
        {
            foreach(PartEditor.PartList partList in PartEditor.AllPartLists)
            {
                SFConfig.SetString("Models", partList.label + " Model", partList.parts[partList.index].partReferenceName);
            }

            foreach (PartEditor.MatList matList in PartEditor.AllMatLists)
            {
                SFConfig.SetString("Materials", matList.label + " Material", matList.mats[matList.index].materialReferenceName);
            }

            foreach (PartEditor.PartAdjustment partAdjustment in PartEditor.AllPartAdjustments)
            {
                SFConfig.SetFloat(partAdjustment.type, partAdjustment.label, partAdjustment.value);
            }

            SFConfig.SetBool("Accessory A", "AccAReverseLeftRot", PartEditor.AccARotReverseLeft);
            SFConfig.SetBool("Accessory A", "AccAReverseRightRot", PartEditor.AccARotReverseRight);

            SFConfig.SetBool("Accessory B", "AccBReverseLeftRot", PartEditor.AccBRotReverseLeft);
            SFConfig.SetBool("Accessory B", "AccBReverseRightRot", PartEditor.AccBRotReverseRight);

            Logger.log.Debug("Saved SaberForge settings");
        }

        public static void LoadConfig()
        {
            //Load parts
            foreach (PartEditor.PartList partList in PartEditor.AllPartLists)
            {
                SetPartListIndexFromConfig(SFConfig.GetString("Models", partList.label + " Model", partList.defaultPartRef, true), partList);
            }

            foreach (PartEditor.MatList matList in PartEditor.AllMatLists)
            {
                SetMatListIndexFromConfig(SFConfig.GetString("Materials", matList.label + " Material", matList.defaultMatRef, true), matList);
            }

            foreach (PartEditor.PartAdjustment partAdjustment in PartEditor.AllPartAdjustments)
            {
                partAdjustment.value = SFConfig.GetFloat(partAdjustment.type, partAdjustment.label, partAdjustment.defaultValue, true);
            }

            PartEditor.AccARotReverseLeft = SFConfig.GetBool("Accessory A", "AccAReverseLeftRot", false, true);
            PartEditor.AccARotReverseRight = SFConfig.GetBool("Accessory A", "AccAReverseRightRot", false, true);

            PartEditor.AccBRotReverseLeft = SFConfig.GetBool("Accessory B", "AccBReverseLeftRot", false, true);
            PartEditor.AccBRotReverseRight = SFConfig.GetBool("Accessory B", "AccBReverseRightRot", false, true);


            Logger.log.Debug("Loaded SaberForge settings");
        }

        //check to ensure saved part actually exists in the loaded assets and sets list index. Might not exist if part bundles are removed
        static void SetPartListIndexFromConfig(string partName, PartEditor.PartList partList)
        {
            int partIndex = -1;

            for (int i = 0; i < partList.parts.Count; i++)
            {
                if (partList.parts[i].partReferenceName == partName)
                {
                    partIndex = i;
                }
            }

            if (partIndex == -1)
            {
                Logger.log.Debug("Could not find saved part in loaded lists! Setting to default");
                partIndex = 0;
            }

            partList.index = partIndex;
        }

        static void SetMatListIndexFromConfig(string matName, PartEditor.MatList matList)
        {
            int matIndex = -1;

            for (int i = 0; i < matList.mats.Count; i++)
            {
                if (matList.mats[i].materialReferenceName == matName)
                {
                    matIndex = i;
                }
            }

            if (matIndex == -1)
            {
                Logger.log.Debug("Could not find saved material in loaded lists! Setting to default");
                matIndex = 0;
            }

            matList.index = matIndex;

        }

    }
}
