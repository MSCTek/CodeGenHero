﻿using System;

namespace CodeGenHero.EAMVCXamPOCO.Xam.ModelData
{
	public abstract class BaseAuditEdit
	{
		public DateTime CreatedDate { get; set; }

		public Guid CreatedUserId { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime UpdatedDate { get; set; }

		public Guid UpdatedUserId { get; set; }
	}
}