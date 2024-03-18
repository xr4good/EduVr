using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGameComponents.SlideComponent
{
    [RequireComponent(typeof(SlideController))]
    public class ButtonHelper : ButtonHelperAbstract
    {
        private SlideController slideController;
        protected override void ChildStart()
        {
            slideController = GetComponent<SlideController>();
        }

        protected override void ChildUpdate()
        {
            int currentSlideIndex = slideController.currentSlideIndex;
            if (slideController.isLastSlide || !slideController.IsSlideAvailable(currentSlideIndex + 1))
            {
                DisableButton("next");
            }
            else
                EnableButton("next");

            if (slideController.isFirstSlide || !slideController.IsSlideAvailable(currentSlideIndex - 1))
                DisableButton("previous");
            else
                EnableButton("previous");
        }
    }
}
