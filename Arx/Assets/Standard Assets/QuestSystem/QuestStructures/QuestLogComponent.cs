﻿using Assets.Standard_Assets._2D.Scripts.Game_State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets._2D.Scripts.Managers;

namespace Assets.Standard_Assets.QuestSystem.QuestStructures
{
    public delegate void QuestEvent(Quest quest);

    public class QuestLogComponent : MonoBehaviour
    {
        private QuestDatabase _instanciatedQuests;

        [SerializeField]
        private QuestDatabase _quests;

        public QuestEvent OnQuestAssigned;

        public Quest GetQuest(string id)
        {
            return _instanciatedQuests.GetQuest(id);
        }

        public bool HasQuestActive(string id)
        {
            var quest = GetQuest(id);
            return quest.QuestStatus == QuestStatus.Active;
        }

        public bool HasQuestActive(Quest quest)
        {
            return HasQuestActive(quest.questId);
        }

        public void GiveQuest(Quest quest)
        {
            quest = _instanciatedQuests.GetQuest(quest.questId);
            if(quest.QuestStatus == QuestStatus.Inactive)
            {
                if(OnQuestAssigned != null)
                {
                    OnQuestAssigned(quest);
                }
                quest.Activate();
            }
            
        }

        public Quest[] GetQuests()
        {
            return _instanciatedQuests.Quests;
        }

        public void SetQuestsStates(QuestState[] questStates)
        {
            for(var idx = 0; idx < questStates.Length; idx++)
            {
                var questState = questStates[idx];
                var quest = GetQuest(questState.QuestId);
                quest.QuestStatus = questState.QuestStatus;
                quest.tasks = questState.Tasks.DeepClone().ToList();
            }
        }

        private void Awake()
        {
            _instanciatedQuests = _quests.Clone();
        }

        private void Start()
        {
            var subscriber = this.gameObject.GetComponent<IQuestSubscriber>();
        }
    }
}
