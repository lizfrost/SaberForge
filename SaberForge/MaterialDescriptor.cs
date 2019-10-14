using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaberForge
{
    class MaterialDescriptor : MonoBehaviour
    {
        public MaterialInfo.MaterialType materialType;
        public string materialDisplayName;
        public bool supportsCustomColors;
    }
}
