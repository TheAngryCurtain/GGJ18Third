using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetInfo
{
    public static bool bShowLogs = true;

    public string InformantName;
    public string Description;
    public ColorInfo AgentHatColor;
    public ColorInfo AgentCoatColor;
    public ColorInfo AgentPantsColor;
    public ColorInfo TargetHatColor;
    public ColorInfo TargetCoatColor;
    public ColorInfo TargetPantsColor;
    public string TargetName;
    public Transform TargetSpawn;
    public int CurrentHintIndex;
    public List<TargetHintInfo> HintCollection;

    public bool DoesMyOutfitMatch()
    {
        bool bHatsOkay = AreColorsSimilar(AgentHatColor.Color, TargetHatColor.Color);
        bool bCoatOkay = AreColorsSimilar(AgentCoatColor.Color, TargetCoatColor.Color);
        bool bPantsOkay = AreColorsSimilar(AgentPantsColor.Color, TargetPantsColor.Color);

        if (bShowLogs)
        {
            Debug.Log("bHatsOkay=" + bHatsOkay);
            Debug.Log("bCoatOkay=" + bCoatOkay);
            Debug.Log("bPantsOkay=" + bPantsOkay);
        }

        return bHatsOkay && bCoatOkay && bPantsOkay;
    }

    public static bool AreColorsSimilar(Color color1, Color color2)
    {
        float minColorPercentForMatch = 0.7f;
        float maxColorPercentForMatch = 1.3f;

        if (bShowLogs)
        {
            Debug.Log("color1=" + color1);
            Debug.Log("color2=" + color2);
        }
        var point1 = 0.01f;

        float blue1Color = (color1.b + point1);
        float green1Color = (color1.g + point1);
        float red1Color = (color1.r + point1);

        float blue2Color = (color2.b + point1);
        float green2Color = (color2.g + point1);
        float red2Color = (color2.r + point1);

        //+0.01 is a hack so we can't divide by zero
        float blueDiff = blue1Color / blue2Color;
        float greenDiff = green1Color / green2Color;
        float RedDiff = red1Color / red2Color;

        if (bShowLogs)
        {
            Debug.Log("blueDiff=" + blueDiff);
            Debug.Log("greenDiff=" + greenDiff);
            Debug.Log("RedDiff=" + RedDiff);
        }

        var blueDelta = blue1Color - blue2Color;
        var greenDelta = green1Color - green2Color;
        var redDelta = red1Color - red2Color;

        if (bShowLogs)
        {
            Debug.Log("blueDelta=" + blueDelta);
            Debug.Log("greenDelta=" + greenDelta);
            Debug.Log("redDelta=" + redDelta);
        }

        bool bIsBlueRight = false;
        bool bIsRedRight = false;
        bool bIsGreenRight = false;

        //we also check against 0.1, because we dont want to fail tiny increments when theyre below off by a couple rbg values like 0.03 to 0.001
        if (((blueDiff > minColorPercentForMatch) && (blueDiff < maxColorPercentForMatch)) || Mathf.Abs(blueDelta) < 0.1)
        {
            bIsBlueRight = true;
        }
        if (((RedDiff > minColorPercentForMatch) && (RedDiff < maxColorPercentForMatch)) || Mathf.Abs(redDelta) < 0.1)
        {
            bIsRedRight = true;
        }
        if (((greenDiff > minColorPercentForMatch) && (greenDiff < maxColorPercentForMatch)) || Mathf.Abs(greenDelta) < 0.1)
        {
            bIsGreenRight = true;
        }

        if (bShowLogs)
        {
            Debug.Log("bIsBlueRight=" + bIsBlueRight);
            Debug.Log("bIsGreenRight=" + bIsGreenRight);
            Debug.Log("bIsRedRight=" + bIsRedRight);
        }

        return bIsBlueRight && bIsRedRight && bIsGreenRight;
    }
}

[Serializable]
public class TargetHintInfo
{
    public string HintText;
    public ColorInfo HintColorInfo;
}

[Serializable]
public class ColorInfo
{
    public eClothingType ClothingType;
    public Color Color;
}

public enum eClothingType
{
    Hat = 0,
    Coat,
    Pants
}

