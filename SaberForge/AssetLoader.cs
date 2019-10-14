using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using Unity;
using UnityEngine;
using UnityEngine.UI;


namespace SaberForge
{

    class AssetLoader : MonoBehaviour
    {
        //Loads in assets exported from Unity, adds them to lists so mod can find them

        public static GameObject SaberForge_PH;
        public static GameObject ForgeProp;


        public static void Init()
        {
            CreatePartLists();
            LoadSaber();

            FindUserTextures();
        }

        //Get list of part bundles from mod assets folder
        public static List<string> RetrievePartBundles()
        {
            List<string> partBundlePaths = (Directory.GetFiles(Path.Combine(Application.dataPath, "../SaberForgeAssets/"), "*.partbundle", SearchOption.AllDirectories).ToList());
            Logger.log.Debug($"Found {partBundlePaths.Count} part bundles");

            return partBundlePaths;
        }

        // compose part lists
        public static void CreatePartLists()
        {
            //find all the part lists in the assets files
            List<GameObject> partBundleObjs = new List<GameObject>();

            foreach (string path in RetrievePartBundles())
            {
                GameObject tempObj = AssetBundle.LoadFromFile(path).LoadAsset<GameObject>("_PartBundle");

                if (tempObj.GetComponent<PartBundleDescriptor>() == null)
                    Logger.log.Debug("No Saber Forge part bundle descriptor on " + tempObj.name.ToString() + " - skipped");

                else
                    partBundleObjs.Add(tempObj);

            }

            //look through each loaded bundle
            foreach (GameObject partBundleObj in partBundleObjs)
            {

                PartBundleDescriptor partBundleDesc = partBundleObj.GetComponent<PartBundleDescriptor>();          

                if (partBundleDesc.forgeProp != null)
                {
                    ForgeProp = partBundleDesc.forgeProp;
                    Logger.log.Debug("Got Forge Prop Hooray!");
                }

                Logger.log.Debug("Grabbing parts from " + partBundleDesc.partBundleName + " by " + partBundleDesc.partBundleAuthor);

                //find all partDesc components and add them to the lists
                PartDescriptor[] partDescs = partBundleObj.GetComponentsInChildren<PartDescriptor>();
                Logger.log.Debug("Found " + partDescs.Length.ToString() + " parts in " + partBundleDesc.partBundleName);

                foreach (PartDescriptor partDesc in partDescs)              
                    AddPartToList(partBundleDesc, partDesc);              

                //now find all mat descs in the part bundle
                MaterialDescriptor[] matDescs = partBundleObj.GetComponentsInChildren<MaterialDescriptor>();
                Logger.log.Debug("Found " + matDescs.Length.ToString() + " materials in " + partBundleDesc.partBundleName);

                foreach (MaterialDescriptor matDesc in matDescs)
                {
                    if (matDesc.gameObject.GetComponent<MeshRenderer>() != null)
                    {

                        AddMaterialToList(partBundleDesc, matDesc, matDesc.gameObject.GetComponent<MeshRenderer>().material);
                    } else
                    {
                        Logger.log.Debug("No mesh renderer to grab a material from on " + matDesc.materialDisplayName + " in " + partBundleDesc.partBundleName + " part bundle, skipping");
                    }
                  
                }               

            }

            //duplicate Accessory List and Secondary Mats List
            PartEditor.AccBList.parts = PartEditor.AccAList.parts;
            PartEditor.TertiaryList.mats = PartEditor.SecondaryList.mats;

        }

        static void AddPartToList(PartBundleDescriptor partBundleDesc, PartDescriptor partDesc)
        {
            PartEditor.PartList partList = null;

            //add to the right list
            switch (partDesc.partType)
            {
                case (PartInfo.PartType.Blade):
                    partList = PartEditor.BladeList;
                    break;
                case (PartInfo.PartType.Guard):
                    partList = PartEditor.GuardList;
                    break;
                case (PartInfo.PartType.FingerGuard):
                    partList = PartEditor.GuardList;
                    break;
                case (PartInfo.PartType.Handle):
                    partList = PartEditor.HandleList;
                    break;
                case (PartInfo.PartType.Pommel):
                    partList = PartEditor.PommelList;
                    break;
                case (PartInfo.PartType.Accessory):
                    partList = PartEditor.AccAList;
                    break;
            }

            //create part key 
            string partRefName = partBundleDesc.partBundleAuthor + "." + partBundleDesc.partBundleName + "." + partDesc.partType.ToString() + "." + partDesc.partName;

            foreach (PartInfo part in partList.parts)
            {
                if (part.partReferenceName == partRefName)
                {
                    Logger.log.Debug("A part with the name " + part.partReferenceName + " has already been added - skipping. Make sure all part and bundle names are unique!");
                    return;
                }
            }

            PartInfo newPart = new PartInfo(partDesc.partType, partDesc.gameObject, partRefName, partDesc.partDisplayName);
            partList.parts.Add(newPart);
            Logger.log.Debug("Added " + partRefName + " to the list of " + partDesc.partType.ToString()); ;

        }

        public static void AddMaterialToList( PartBundleDescriptor partBundleDesc, MaterialDescriptor matDesc, Material mat)
        {
            PartEditor.MatList matList = null;

            switch (matDesc.materialType)
            {
                case (MaterialInfo.MaterialType.Glow):
                    matList = PartEditor.GlowList;
                    break;
                case (MaterialInfo.MaterialType.Secondary):
                    matList = PartEditor.SecondaryList;
                    break;
                case (MaterialInfo.MaterialType.Trail):
                    matList = PartEditor.TrailList;
                    break;
                case (MaterialInfo.MaterialType.NamePlate):
                    matList = PartEditor.NamePlateList;
                    break;
                case (MaterialInfo.MaterialType.Template):
                    matList = PartEditor.TemplateList;
                    break;

            }

            string trimmedName = mat.name.Replace(" (Instance)", "");

            string matRefName = partBundleDesc.partBundleAuthor + "." + partBundleDesc.partBundleName + "." + matDesc.materialType.ToString() + "." + trimmedName;

            foreach (MaterialInfo listMatInfo in matList.mats)
            {
                if (listMatInfo.materialReferenceName == matRefName)
                {
                    Logger.log.Debug("A material with the name " + listMatInfo.materialReferenceName + " has already been added - skipping. Make sure all material and bundle names are unique!");
                    return;
                }
            }

            MaterialInfo matInfo = new MaterialInfo(matDesc.materialType, mat, matRefName, matDesc.materialDisplayName, matDesc.supportsCustomColors);

            matList.mats.Add(matInfo);
            Logger.log.Debug("Added " + matRefName + " to the list of " + matInfo.materialType.ToString() + " materials");

        }

        public static void LoadSaber()
        {
            // only works if custom sabers loads first..., create a co-routine???
            GameObject tempSaber = Resources.FindObjectsOfTypeAll<SaberDescriptor>().Last(x => (x.SaberName == "SaberForge")).gameObject;

            if (tempSaber == null)
            {
                Logger.log.Debug("Can't find SaberForge place holder asset!");
                return;
            }

            SaberForge_PH = tempSaber;
            Logger.log.Debug("Found " + SaberForge_PH.name.ToString());
        }


        //find user textures and send them to be loaded into custom materials
        public static void FindUserTextures()
        {
            List<string> imagePaths = (Directory.GetFiles(Path.Combine(Application.dataPath, "../SaberForgeAssets/UserTextures/"), "*.png", SearchOption.AllDirectories).ToList());

            Logger.log.Debug($"Found {imagePaths.Count} user textures");


            foreach (string path in imagePaths)
            {
                string fileName = path.Replace(Path.Combine(Application.dataPath, "../SaberForgeAssets/UserTextures/"), "");

                string[] fileInfo = fileName.Split('_');

                if (fileInfo.Length != 3)
                {
                    Logger.log.Debug("Invalid texture name " + fileName + ", please refer to user guide for texture naming guidelines");
                    continue ;
                }

                bool customColor = false;

                if(fileInfo[1] == "cctrue")
                {
                    customColor = true;
                } else if (fileInfo[1] == "ccfalse")
                {
                    customColor = false;
                } else
                {
                    Logger.log.Debug("Invalid texture name " + fileName + ", please refer to user guide for texture naming guidelines");
                    continue;
                }

                string textureName = fileInfo[2].Replace(".png", "");
                Material templateMat;
                MaterialInfo.MaterialType matType = MaterialInfo.MaterialType.NamePlate;



                //find template material using file name
                if (fileInfo[0] == "cutout")
                {
                    textureName += "_Cutout";

                    if (customColor)
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Cutout_Demo_CC", PartEditor.TemplateList);

                    else
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Cutout_Demo_NoCC", PartEditor.TemplateList);
                }
                else if (fileInfo[0] == "trans")
                {
                    textureName += "_Trans";

                    if (customColor)
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Trans_Demo_CC", PartEditor.TemplateList);
                    else
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Trans_Demo_NoCC", PartEditor.TemplateList);

                }
                else if (fileInfo[0] == "opaque")
                {
                    textureName += "_Opaque" ;

                    if (customColor)
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Opaque_Demo_CC", PartEditor.TemplateList);
                    else
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Opaque_Demo_NoCC", PartEditor.TemplateList);

                } else if (fileInfo[0] == "trail")
                {
                    textureName += "_Trail";
                    matType = MaterialInfo.MaterialType.Trail;

                    if (customColor)
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Basic_Trail_CC", PartEditor.TemplateList);
                    else
                        templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Basic_Trail_NoCC", PartEditor.TemplateList);
                }
                 else if (fileInfo[0] == "flagtrail")
            {
                textureName += "_FlagTrail";
                matType = MaterialInfo.MaterialType.Trail;

                if (customColor)
                    templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Flag_Trail_CC", PartEditor.TemplateList);
                else
                    templateMat = PartEditor.FindMaterialInList("FrostDragon.CoreParts.Template.Flag_Trail_NoCC", PartEditor.TemplateList);
            }
                 else
                {
                    Logger.log.Debug("Invalid texture name " + fileName + ", please refer to user guide for texture naming guidelines");
                    continue;
                }

                GenerateUserMaterial(path, textureName, templateMat, customColor, matType);

            }

            //update labels
            UIFunctions.UpdateAllMatLabels();


        }

        public static void GenerateUserMaterial(string texturePath, string textureName, Material templateMat, bool supportsCustomColors, MaterialInfo.MaterialType matType)
        {
            //stolen from http://gyanendushekhar.com/2017/07/08/load-image-runtime-unity/
            byte[] byteArray = File.ReadAllBytes(texturePath);
            //create a texture and load byte array to it
            // Texture size does not matter 
            Texture2D sampleTexture = new Texture2D(2, 2);
            // the size of the texture will be replaced by image size
            bool isLoaded = sampleTexture.LoadImage(byteArray);
            // apply this texure as per requirement on image or material
            GameObject image = GameObject.Find("RawImage");

            Material newMat = Instantiate(templateMat);


            newMat.name = textureName;

            if (isLoaded)
            {
                newMat.SetTexture("_Tex", sampleTexture);

                //Logger.log.Debug("Loaded user texture " + textureName);


                PartBundleDescriptor newBundleDesc = new PartBundleDescriptor();
                newBundleDesc.partBundleAuthor = "User";
                newBundleDesc.partBundleName = "UserTextures";

                MaterialDescriptor newMatDesc = new MaterialDescriptor();

                newMatDesc.materialType = matType;
                newMatDesc.materialDisplayName = textureName;
                newMatDesc.supportsCustomColors = supportsCustomColors;

                AssetLoader.AddMaterialToList(newBundleDesc, newMatDesc, newMat);


            }
        }

    }
}
