// <auto-generated> - Template:ModelsBackedByDto, Version:1.0, Id:f1539c0d-024f-4b1f-b346-132cdd9dd31f
using CodeGenHero.Logging;
using CodeGenHero.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using CodeGenHero.BingoBuzz.API.Client.Interface;
using CodeGenHero.BingoBuzz.Model.BB.Interface;
using xDTO = CodeGenHero.BingoBuzz.DTO.BB;

namespace CodeGenHero.BingoBuzz.Model.BB
{

	public class LoadRequestBingoInstanceEventType : EventArgs
	{
		public LoadRequestBingoInstanceEventType(string propertyNameRequestingLoad)
		{
			PropertyNameRequestingLoad = propertyNameRequestingLoad;
		}

		public string PropertyNameRequestingLoad { get; set; }
	}


	public partial class BingoInstanceEventType : BaseModel<IWebApiDataServiceBB>, IBingoInstanceEventType
	{
		public event EventHandler<LoadRequestBingoInstanceEventType> OnLazyLoadRequest = delegate { }; // Empty delegate. Thus we are sure that value is always != null because no one outside of the class can change it.
		private xDTO.BingoInstanceEventType _dto = null;

		public BingoInstanceEventType(ILoggingService log, IDataService<IWebApiDataServiceBB> dataService) : base(log, dataService)
		{
			_dto = new xDTO.BingoInstanceEventType();
			OnLazyLoadRequest += HandleLazyLoadRequest;
		}

		public BingoInstanceEventType(ILoggingService log, IDataService<IWebApiDataServiceBB> dataService, xDTO.BingoInstanceEventType dto) : this(log, dataService)
		{
			_dto = dto;
		}


		public virtual int BingoInstanceEventTypeId { get { return _dto.BingoInstanceEventTypeId; } }
		public virtual string Name { get { return _dto.Name; } }

		private List<IBingoInstanceEvent> _bingoInstanceEvents = null; // Reverse Navigation


		public virtual List<IBingoInstanceEvent> BingoInstanceEvents
		{
			get
			{
				if (_bingoInstanceEvents == null)
				{
					OnLazyLoadRequest(this, new LoadRequestBingoInstanceEventType(nameof(BingoInstanceEvents)));
				}

				return _bingoInstanceEvents;
			}
		}



	}
}
