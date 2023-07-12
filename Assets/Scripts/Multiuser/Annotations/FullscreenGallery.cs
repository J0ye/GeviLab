using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenGallery : FullscreenAnnotation
{
    public List<Annotation> contentSlides = new List<Annotation>();

    public Image contentImage;
    public TMP_Text contentNote;

    protected int activeSlideIndex = 0;

    protected override void Start()
    {
        UpdateDisplayToActiveSlide();
        delayed = true;
        if (origin != null) origin.SetDisplay(false);
    }

    public void ScrollDown()
    {
        activeSlideIndex++;
        if(activeSlideIndex >= contentSlides.Count)
        {
            activeSlideIndex = 0;
        }

        UpdateDisplayToActiveSlide();
    }

    public void ScrollUp()
    {
        activeSlideIndex--;
        if(activeSlideIndex < 0)
        {
            activeSlideIndex = contentSlides.Count - 1;
        }

        UpdateDisplayToActiveSlide();
    }

    public void UpdateDisplayToActiveSlide()
    {
        if(contentSlides.Count <= 0)
        {
            return;
        }

        Annotation activeAnnotation = contentSlides[activeSlideIndex];
        switch (activeAnnotation.annotationType)
        {
            case AnnotationType.Image:
                ImageAnnotation imageSlide = (ImageAnnotation)contentSlides[activeSlideIndex];
                DisplayImage(imageSlide);
                break;
            case AnnotationType.Note:
                Note noteSlide = (Note)contentSlides[activeSlideIndex];
                DisplayNote(noteSlide);
                break;
            default:
                Debug.Log("Not a displayable annotation type. Type is " + activeAnnotation.annotationType);
                break;
        }
    }

    protected void DisplayImage(ImageAnnotation target)
    {
        SlideDisplayObject(true);
        contentImage.sprite = target.content;
        //Fit image to size
        float tempWidth = target.content.texture.width;
        float tempHeigth = target.content.texture.height;
        contentImage.GetComponent<AspectRatioFitter>().aspectRatio = tempWidth / tempHeigth;
    }

    protected void DisplayNote(Note target)
    {
        SlideDisplayObject(false);
        contentNote.text = target.text;
    }

    protected void SlideDisplayObject(bool doImage)
    {
        contentImage.gameObject.SetActive(doImage);
        contentNote.gameObject.SetActive(!doImage);
    }
}
