using System.Collections.Generic;
using UnityEngine;

namespace SaberForge
{
    public class PartBundleDescriptor : MonoBehaviour
    {
        //set in Unity Editor when exporting parts
        public string partBundleName;
        public string partBundleAuthor;

        public GameObject forgeProp;
    }
}
