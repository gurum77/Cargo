using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelector : MonoBehaviour {

    public Image selectedImage;
    public Text selectedImageTitleText;

    public Sprite[] images;
    public string[] titles;
    public int SelectedImageIndex
    { get; set; }

   

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        // 데이타 유효성 체크
        if(images.Length != titles.Length)
        {
            Debug.Assert(false);
            return;
        }

        RefreshSelectedImage();
    }

    public void SelectImage(int index)
    {
        SelectedImageIndex = index;
        
    }

    // 선택된 이미지 갱신
    // 타이틀등의 부가정보도 모두 갱신한다.
    void RefreshSelectedImage()
    {
        if(selectedImage && SelectedImageIndex > -1 && SelectedImageIndex < images.Length)
        {
            selectedImage.sprite = images[SelectedImageIndex];
        }

        if(selectedImageTitleText && SelectedImageIndex > -1 && SelectedImageIndex < titles.Length)
        {
            selectedImageTitleText.text = titles[SelectedImageIndex];
        }
        
    }

    public void OnLeftButtonClicked()
    {
        SelectedImageIndex = SelectedImageIndex - 1;
        if (SelectedImageIndex < 0)
            SelectedImageIndex = 0;

        RefreshSelectedImage();
    }

    public void OnRightButtonClicked()
    {
        SelectedImageIndex = SelectedImageIndex + 1;
        if (SelectedImageIndex >= images.Length)
            SelectedImageIndex = images.Length - 1;

        RefreshSelectedImage();
    }

    public void OnSelectButtonClicked()
    {

    }
}
