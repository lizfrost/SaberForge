using System.Collections.Generic;
using UnityEngine;

namespace SaberForge
{
    public class PartDescriptor : MonoBehaviour
    {
        //set in Unity Editor when exporting parts
        public string partName;
        public string partDisplayName;
        public PartInfo.PartType partType;

        //assign part sub-objects in Unity to ensure they have the correct material
        public List<GameObject> glowMatObjects;
        public List<GameObject> secondaryMatObjects;
        public List<GameObject> tertiaryMatObjects;
        public List<GameObject> namePlateMatObjects;
    }
}
