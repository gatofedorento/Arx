﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Inventory
{
    public interface IItemUseFinderController
    {
        IEnumerable<IItemUseTrigger> ItemUseTriggers { get; }
    }
}
