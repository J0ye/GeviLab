using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryAnnotation : ImageAnnotation
{
    [Header("Gallery Settings")]
    public List<Annotation> slides = new List<Annotation>();

    /// <summary>
    /// </summary>
    public override void Open()
    {
        GameObject newFullscreenGallery = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        FullscreenGallery fg = newFullscreenGallery.GetComponent<FullscreenGallery>();
        fg.contentSlides = slides;
        fg.UpdateDisplayToActiveSlide(); //Update display of fullscreen


        fg.origin = this;
        GameState.instance.SetActivePlayerControls(false);
    }

    public override void OpenXR()
    {
        GameObject newFullscreenGallery = Instantiate(fullscreenPrefabXR, 
            GetPositionInFrontOfAnnotation(), Quaternion.identity);
        newFullscreenGallery.transform.DOScale(newFullscreenGallery.transform.localScale.x * xRPrefabSize, xRPrefabAnimationDuration);
        FullscreenGallery fg = newFullscreenGallery.GetComponent<FullscreenGallery>();
        fg.contentSlides = slides;
        fg.UpdateDisplayToActiveSlide(); //Update display of fullscreen


        fg.origin = this;
        GameState.instance.SetActivePlayerControls(false);
    }
}
