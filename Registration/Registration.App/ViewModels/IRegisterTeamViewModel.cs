﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ViewModels
{
	public interface IRegisterTeamViewModel
	{
        event EventHandler CloseViewRequested;

        Guid ContestId { get; set; }
    }
}
