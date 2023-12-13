using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LodingImgTxtConroller : MonoBehaviour
{
    public Sprite[] images; // 에디터에서 설정하거나 Resources 폴더에서 로드
    public Image displayImage; // 바꿀 이미지가 있는 UI 컴포넌트

    void Start()
    {
       
        int randomIndex = Random.Range(0, images.Length);   
        displayImage.sprite = images[randomIndex];
    }
}
