﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Commands
{
    public class MakePlayerShot : SinglePlayerGameCommand
    {
        public int ShotNumber { get; set; }
        public int Score { get; set; }
    }
}