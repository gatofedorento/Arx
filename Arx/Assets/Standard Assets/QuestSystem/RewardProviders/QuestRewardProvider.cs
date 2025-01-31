﻿using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.RewardProviders
{
    [Serializable]
    public class QuestRewardProvider : ScriptableObject, IRewardProvider
    {
        public Quest _quest;

        public void GiveReward()
        {
            FindObjectOfType<QuestLogComponent>().GiveQuest(_quest);
        }
    }
}
