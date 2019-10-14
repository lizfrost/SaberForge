using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SaberForge
{

    public class MaterialInfo : MonoBehaviour

    {
        public enum MaterialType { Glow, Secondary, Trail, NamePlate, Template }

        public MaterialType materialType;
        public Material material;
        public string materialReferenceName;
        public string materialDisplayName;
        public bool supportsCustomColors;

        public MaterialInfo(MaterialType type, Material mat, string refName, string displayName, bool cc)
        {
            materialType = type;
            material = mat;
            materialReferenceName = refName;
            materialDisplayName = displayName;
            supportsCustomColors = cc;
        }


    }
}
