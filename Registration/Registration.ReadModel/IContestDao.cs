﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public interface IContestDao
	{
		Sequencing FindSequencing(Guid contestId);
	}
}