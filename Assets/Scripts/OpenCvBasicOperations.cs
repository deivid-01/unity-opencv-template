﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

public class OpenCvBasicOperations : MonoBehaviour
{
    public List<SpriteRenderer> imagesBase;
    public List<Texture2D> textures ;
    List<Mat> matResults;

    List<Mat> mats = new List<Mat>();
    void Start()
    {
        SetMats();
        //---------------------------------------------
        //Sum
        // Mat matR = mat + mat2;  //or Cv2.Sum(mat, mat2, dst, null,-1);
        //--------------------------------------------
        //Crop Image
        // Mat matR =mat[new OpenCvSharp.Rect(0, 0, mat.Width/2, mat.Height)]; 
        //-----------------------------------------
        //Get type of Mat
        //print(mat.Type().ToString());

        Mat[] channels = mats[0].Split();

        Mat mainMat = (0.1 * channels[0] + 0.6 * channels[1] + 0.3 * channels[2]);


        for (int i = 0; i < imagesBase.Count; i++)
        {
            imagesBase[i].sprite = CreateNewSprite(matResults[i]);
        }

        imagesBase.ForEach(imageBase=> imageBase.sprite = CreateNewSprite(mainMat));




    }
    public void SetMats()
    {
        //mainMat = OpenCvSharp.Unity.TextureToMat(imagesBase.sprite.texture);
        imagesBase.ForEach(imagesBase=>matResults.Add(new Mat()));
      
        textures.ForEach(texture => mats.Add(OpenCvSharp.Unity.TextureToMat(texture)));
    
    }

    public Sprite CreateNewSprite(Mat dst) => Sprite.Create(OpenCvSharp.Unity.MatToTexture(dst), //Texture 
                                                new UnityEngine.Rect(0, 0, dst.Width, dst.Height),//Rect Propierties
                                                new Vector2(0.5f, 0.5f),                          //Offset
                                                100);                                             //Size
    public Mat Sum(Mat mat, Mat mat2) => mat + mat2;
    public Mat Subtr(Mat mat, Mat mat2) => mat - mat2;

    public Mat GrayScale(Mat mat)
    {
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);

        return mat;
    }

    public Mat HideChannel(Mat mat,int  i)
    {
        Mat[] channels = new Mat[3];

        //Separate each channel
        channels = Cv2.Split(mat);
        //BGR iMAGE
        //channel [0] -> Blue
        //channel [1] -> Green
        //channel [2] -> Red

        //Set channel to Zero
        channels[i] = Mat.Zeros(mat.Rows, mat.Cols, MatType.CV_8UC1);

        //Merging Channels
        Cv2.Merge(channels, mat);



        //Update Sprite
        return mat;
    }
    //Show intensity of specific channel( R,G,B)
    Mat ShowSingleChannel(Mat mat, int i) => mat.ExtractChannel(i);
}
