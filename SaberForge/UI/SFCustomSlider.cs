using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using CustomUI.BeatSaber;
using CustomUI.UIElements;
using System.Collections;
using System;
using System.Collections.Generic;
using HMUI;
using System.Linq;

namespace SaberForge
{
    public class SFCustomSlider : CustomSlider
    {
        //this class exists only to try and force the slider text to be formated as a float not as time. 
        //when i work out a better way I will nuke this from orbit

        void OnEnable()
        {
            // UIFunctions.UpdateSliderLabel(this, this.Scrollbar.value);
           // StartCoroutine(SetInitialText());
        }


        //i hate everyone
        public IEnumerator SetInitialText()
        {
            UIFunctions.UpdateSliderLabel(this, this.Scrollbar.value);
            yield return new WaitForSeconds(0.1f); 
            UIFunctions.UpdateSliderLabel(this, this.Scrollbar.value);

        }

    }
}
