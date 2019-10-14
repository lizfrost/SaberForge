using UnityEngine;
using UnityEngine.SceneManagement;
using CustomSaber;
using System.Linq;
using System.Collections.Generic;

namespace SaberForge
{
    class SaberController : MonoBehaviour
    {
        //script placed on actual sabers to handle mesh and material swaps, transform changes.
        GameObject blade;
        GameObject guard;
        GameObject handle;
        GameObject pommel;
        GameObject accA;
        GameObject accB;

        CustomTrail trail;

        public bool isLeft;

        GameObject trailQuad;

        float handlePosOffset;
        float pommelPosOffset;

        PartDescriptor[] allPartDescs;

        ColorManager colorManager;

        void Awake()
        {
            SetReferences();

            UpdatePartModels();
            AdjustParts();
            UpdatePartMaterials();
            UpdateTrail();

            PartEditor.onPartModelChange += UpdatePartModels;
            PartEditor.onPartAdjusted += AdjustParts;
            PartEditor.onMaterialChange += UpdatePartMaterials;

            PartEditor.onTrailChange += UpdateTrail;

        }

        void Update()
        {
            RotateParts();
        }

        void OnDestroy()
        {
            PartEditor.onPartModelChange -= UpdatePartModels;
            PartEditor.onPartAdjusted -= AdjustParts;
            PartEditor.onMaterialChange -= UpdatePartMaterials;
            PartEditor.onTrailChange -= UpdateTrail;
        }

        void SetReferences()
        {
            blade = gameObject.transform.Find("Blade").gameObject;
            guard = gameObject.transform.Find("Guard").gameObject;
            handle = gameObject.transform.Find("Handle").gameObject;
            pommel = gameObject.transform.Find("Pommel").gameObject;
            accA = gameObject.transform.Find("AccessoryA").gameObject;
            accB = gameObject.transform.Find("AccessoryB").gameObject;

            handlePosOffset = handle.transform.localPosition.z;
            pommelPosOffset = pommel.transform.localPosition.z;

            trail = gameObject.GetComponent<CustomTrail>();

            colorManager = Resources.FindObjectsOfTypeAll<ColorManager>().FirstOrDefault();

            if (gameObject.name == "LeftSaber")
                isLeft = true;
            else
                isLeft = false;

            //flip left ??
        }

        void RotateParts()
        {
            //to take start angle into account
            float rotA = (PartEditor.AccARotSpeed.value * Time.time) + PartEditor.AccAAngle.value;
            float rotB = (PartEditor.AccBRotSpeed.value * Time.time) + PartEditor.AccBAngle.value;

            //to be implemented
            //if (isLeft && PartEditor.AccARotReverseLeft)
            //    rotA = -rotA;
            //else if (PartEditor.AccARotReverseRight)
            //    rotA = -rotA;

            //if (isLeft && PartEditor.AccBRotReverseLeft)
            //    rotB = -rotB;
            //else if (PartEditor.AccBRotReverseRight)
            //    rotB = -rotB;

            accA.transform.localRotation = Quaternion.Euler(0, 0, rotA);
            accB.transform.localRotation = Quaternion.Euler(0, 0, rotB);

        }

        public void UpdatePartModels()
        {
            //currently replacing all parts on a single part change because laziness
            ReplacePart(blade, PartEditor.BladeList.parts[PartEditor.BladeList.index].partObject);
            ReplacePart(guard, PartEditor.GuardList.parts[PartEditor.GuardList.index].partObject);
            ReplacePart(handle, PartEditor.HandleList.parts[PartEditor.HandleList.index].partObject);
            ReplacePart(pommel, PartEditor.PommelList.parts[PartEditor.PommelList.index].partObject);
            ReplacePart(accA, PartEditor.AccAList.parts[PartEditor.AccAList.index].partObject);
            ReplacePart(accB, PartEditor.AccBList.parts[PartEditor.AccBList.index].partObject);

            AdjustParts();
        }

        private void ReplacePart(GameObject parentObj, GameObject newPartObj)
        {
            int childCount = parentObj.transform.childCount;

            //destroy any old parts
            for (int i = 0; i < childCount; i++)
            {
                Destroy(parentObj.transform.GetChild(i).gameObject);
            }

            //create new part
            GameObject newObj = Instantiate(newPartObj, parentObj.transform);

            // set materials;
            IteratePartDescriptorMaterials(newObj.GetComponent<PartDescriptor>());

        }

        void UpdatePartMaterials()
        {
            allPartDescs = GetComponentsInChildren<PartDescriptor>();


            //currently replacing all mats on a single mat change
            foreach (PartDescriptor partDesc in allPartDescs)
            {
                IteratePartDescriptorMaterials(partDesc);
            }

            trail.TrailMaterial = PartEditor.TrailList.mats[PartEditor.TrailList.index].material;

            if (trailQuad != null)
            {
                trailQuad.GetComponent<MeshRenderer>().material = trail.TrailMaterial;
            }

        }

        void IteratePartDescriptorMaterials(PartDescriptor partDesc)
        {

            foreach (GameObject partObj in partDesc.glowMatObjects)
                SetMaterial(partObj, PartEditor.GlowList.mats[PartEditor.GlowList.index].material);

            foreach (GameObject partObj in partDesc.secondaryMatObjects)
                SetMaterial(partObj, PartEditor.SecondaryList.mats[PartEditor.SecondaryList.index].material);

            foreach (GameObject partObj in partDesc.tertiaryMatObjects)
                SetMaterial(partObj, PartEditor.TertiaryList.mats[PartEditor.TertiaryList.index].material);

            foreach (GameObject partObj in partDesc.namePlateMatObjects)
                SetMaterial(partObj, PartEditor.NamePlateList.mats[PartEditor.NamePlateList.index].material);

        }

        void SetMaterial(GameObject partObj, Material newMat)
        {
            MeshRenderer rend = partObj.GetComponent<MeshRenderer>();

            if (rend != null)
            {
                if (newMat == null)
                    Logger.log.Debug("no mat for " + partObj.name);
                else
                    ApplyMaterials(rend, newMat, GetSaberColor());
            }


            else
                Logger.log.Debug("No mesh renderer component on " + partObj.name + " could not set material. Part incorrectly configured");
        }

        Color GetSaberColor()
        {

            Color saberCol;

            if (colorManager != null)
            {
                if (isLeft)
                    saberCol = colorManager.ColorForSaberType(Saber.SaberType.SaberA);
                else
                    saberCol = colorManager.ColorForSaberType(Saber.SaberType.SaberB);
            }
            else
            {
                if (isLeft)
                    saberCol = new Color(0.659f, 0.125f, 0.125f, 1);
                else
                    saberCol = new Color(0.125f, 0.392f, 0.659f, 1);
            }

            return saberCol;
        }

        void ApplyMaterials(MeshRenderer rend, Material mat, Color saberCol)
        {
            Material newMat = Instantiate(mat);

            //for consistency with custom sabers
            if (newMat.HasProperty("_Glow") && newMat.GetFloat("_Glow") > 0 || newMat.HasProperty("_Bloom") && newMat.GetFloat("_Bloom") > 0)
            {
                newMat.SetColor("_Color", saberCol);
            }

            rend.material = newMat;
        }


        public void AdjustParts()
        {
            //check and adjust transforms on parts 
            if (blade.transform.localScale.x != PartEditor.BladeXScale.value)
                blade.transform.localScale = new Vector3(PartEditor.BladeXScale.value, blade.transform.localScale.y, 1);

            if (blade.transform.localScale.y != PartEditor.BladeYScale.value)
                blade.transform.localScale = new Vector3(blade.transform.localScale.x, PartEditor.BladeYScale.value, 1);

            if (handle.transform.localScale.x != PartEditor.HandleScale.value)
                handle.transform.localScale = new Vector3(PartEditor.HandleScale.value, PartEditor.HandleScale.value, handle.transform.localScale.z);

            SetHandleLength(PartEditor.HandleLength.value);

            SetGuardScale(PartEditor.GuardScale.value);

            if (guard.transform.localRotation.z != PartEditor.GuardAngle.value)
                guard.transform.localRotation = Quaternion.Euler(0, 0, PartEditor.GuardAngle.value);

            float pommelScale = PartEditor.PommelScale.value;
            if (pommel.transform.localScale.x != pommelScale)
                pommel.transform.localScale = new Vector3(pommelScale, pommelScale, pommelScale);


            accA.transform.localPosition = new Vector3(0, 0, PartEditor.AccAPos.value);
            accB.transform.localPosition = new Vector3(0, 0, PartEditor.AccBPos.value);

            float accAScale = PartEditor.AccAScale.value;
            accA.transform.localScale = new Vector3(accAScale, accAScale, accAScale);

            float accBScale = PartEditor.AccBScale.value;
            accB.transform.localScale = new Vector3(accBScale, accBScale, accBScale);

            if (PartEditor.AccARotSpeed.value == 0)
                accA.transform.localRotation = Quaternion.Euler(0, 0, PartEditor.AccAAngle.value);

            if (PartEditor.AccBRotSpeed.value == 0)
                accB.transform.localRotation = Quaternion.Euler(0, 0, PartEditor.AccBAngle.value);

        }


        void SetHandleLength(float newScale)
        {
            handle.transform.localScale = new Vector3(handle.transform.localScale.x, handle.transform.localScale.y, newScale);

            //adjust pommel position to match handle length
            float pommelDistance = handlePosOffset - pommelPosOffset;
            float pommelDistScale = pommelDistance * newScale;

            pommel.transform.localPosition = new Vector3(0, 0, handlePosOffset - pommelDistScale);

            //adjust guard Z scale only if its a full length finger guard
            if (PartEditor.GuardList.parts[PartEditor.GuardList.index].partType == PartInfo.PartType.FingerGuard)
                guard.transform.localScale = new Vector3(guard.transform.localScale.x, guard.transform.localScale.y, newScale);
        }

        void SetGuardScale(float newScale)
        {
            //don't adjust guard Z scale if its a full length finger guard
            if (PartEditor.GuardList.parts[PartEditor.GuardList.index].partType == PartInfo.PartType.FingerGuard)
                guard.transform.localScale = new Vector3(newScale, newScale, guard.transform.localScale.z);

            else
                guard.transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        void UpdateTrail()
        {
            //Check and update trail
            trail.PointStart.localPosition = new Vector3(0, 0, PartEditor.TrailStart.value);
            trail.PointEnd.localPosition = new Vector3(0, 0, PartEditor.TrailEnd.value);

            if (trailQuad != null)
            {
                Destroy(trailQuad);
                trailQuad = CreateQuad.CreateTrailQuad(trail.PointStart, trail.PointEnd, trail, colorManager.ColorForSaberType(Saber.SaberType.SaberB));
            }
        }

        public void SpawnTrailQuad()
        {
            trailQuad = CreateQuad.CreateTrailQuad(trail.PointStart, trail.PointEnd, trail, colorManager.ColorForSaberType(Saber.SaberType.SaberB));
        }

    }
}

