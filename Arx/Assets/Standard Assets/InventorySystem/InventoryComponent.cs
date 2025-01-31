﻿using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.InventorySystem
{
    public class InventoryComponent : MonoBehaviour
    {
        private IItemOwner _owner;
        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            _owner = this.gameObject.GetComponent<IItemOwner>();
            Inventory = new Inventory(_owner);
        }
    }
}
