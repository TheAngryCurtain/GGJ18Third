using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUpdatedEvent : GameEvent
{
    public ColorInfo ColorInfo;
}

public class TargetManager : Singleton<TargetManager>
{
    static private System.Random rnd = new System.Random();
    public List<TargetInfo> TargetCollection;
    public TargetInfo CurrentTarget;

    public CharacterCreationData AgentData;
    public CharacterCreationData TargetData;

    public int iHatID;
    public int iTopID;
    public int iBottomID;

    public void Init()
    {
        GenerateTarget();

        //Save off Agent Data
        AgentData = new CharacterCreationData();
        AgentData.lID[ClothesHolder.HATS_INDEX] = 0;
        AgentData.lColor[ClothesHolder.HATS_INDEX] = CurrentTarget.AgentHatColor.Color;

        AgentData.lID[ClothesHolder.TOPS_INDEX] = 0;
        AgentData.lColor[ClothesHolder.TOPS_INDEX] = CurrentTarget.AgentCoatColor.Color;

        AgentData.lID[ClothesHolder.BOTTOMS_INDEX] = 0;
        AgentData.lColor[ClothesHolder.BOTTOMS_INDEX] = CurrentTarget.AgentPantsColor.Color;

        //Save off Target Data
        TargetData = new CharacterCreationData();
        TargetData.lID[ClothesHolder.HATS_INDEX] = 0;
        TargetData.lColor[ClothesHolder.HATS_INDEX] = CurrentTarget.TargetHatColor.Color;

        TargetData.lID[ClothesHolder.TOPS_INDEX] = 0;
        TargetData.lColor[ClothesHolder.TOPS_INDEX] = CurrentTarget.TargetCoatColor.Color;

        TargetData.lID[ClothesHolder.BOTTOMS_INDEX] = 0;
        TargetData.lColor[ClothesHolder.BOTTOMS_INDEX] = CurrentTarget.TargetPantsColor.Color;

        iHatID = Random.RandomRange(0, Game.MAX_NPC) % Game.MAX_NPC;
        iTopID = Random.RandomRange(0, Game.MAX_NPC) % Game.MAX_NPC;
        iBottomID = Random.RandomRange(0, Game.MAX_NPC) % Game.MAX_NPC;
    }

    public void GenerateTarget()
    {
        if(TargetCollection.Count > 0)
        {
            int r = rnd.Next(TargetCollection.Count);

            CurrentTarget = TargetCollection[r];
            CurrentTarget.CurrentHintIndex = -1;
        }
        else
        {
            Debug.LogError("No More Targets");
        }
    }

    public void FoundTarget()
    {
        TargetCollection.Remove(CurrentTarget);
        CurrentTarget = null;
    }

    public TargetHintInfo GetNextHint()
    {
        int curHintIndex = CurrentTarget.CurrentHintIndex;

        if(curHintIndex < CurrentTarget.HintCollection.Count)
        {
            curHintIndex++;
            return CurrentTarget.HintCollection[curHintIndex];
        }

        return null;
    }
}