using UnityEngine;

namespace SaberForge
{
    public class PartInfo
    {
        //for handling part data internally

        public enum PartType { Blade, Guard, FingerGuard, Handle, Pommel, Accessory }

        public PartType partType;
        public GameObject partObject;
        public string partReferenceName;
        public string partDisplayName;

        public PartInfo(PartType type, GameObject gameObject, string refName, string displayName)
        {
            partType = type;
            partObject = gameObject;
            partReferenceName = refName;
            partDisplayName = displayName;
        }
    }
}
