using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace SaberForge
{
    class EditorController : MonoBehaviour
    {

        GameObject saberPreview;
        GameObject forge;


        public void OnOpenEditor()
        {
            //create a previewSaber & forge prop 
            if (forge == null)
            {
                forge = Instantiate(AssetLoader.ForgeProp);

                forge.transform.position = new Vector3(0f, 0f, 0.8f);
                forge.transform.rotation = Quaternion.Euler(0, -90, 0);
            }

            if (saberPreview == null)
            {
                saberPreview = Instantiate(AssetLoader.SaberForge_PH, forge.transform.Find("PreviewSaberPos").transform);

                SaberController[] sabers = saberPreview.GetComponentsInChildren<SaberController>();

                foreach (SaberController sab in sabers)
                {
                    if (sab.isLeft)
                    {
                        sab.transform.Translate(new Vector3(-0.05f, 0.15f, 0.34f));
                        sab.transform.Rotate(new Vector3(0, 180, 0));                     
                    }
                    else
                    {
                        sab.transform.Translate(new Vector3(-0.05f, -0.05f, -0.34f));
                        sab.SpawnTrailQuad();
                    }
                }
           }

        }

        public void OnCloseEditor()
        {
            Destroy(forge);
            Destroy(saberPreview);
        }



    }
}
