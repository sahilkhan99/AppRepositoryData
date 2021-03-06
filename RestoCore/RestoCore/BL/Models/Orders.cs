﻿using System;
using SQLite;

namespace RestoCore
{
	public class Orders : IBusinessEntity
	{
		public Orders()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int CustomerID { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime OrderRequired { get; set; }
		public bool OrderCompleted { get; set; }
		public bool OrderCanceled { get; set; }

		[Indexed]
		public int BaseOrderID { get; set; }
	}
}

