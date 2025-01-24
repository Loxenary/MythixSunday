using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DraggedObject
{
    public RectTransform objectPlaceholder;
    public Transform canvasParent;

    public RectTransform previewObject;

    public Image Image{ get; private set;}

    public void SetImage(Image image){
        Image = image;
        
    }

}