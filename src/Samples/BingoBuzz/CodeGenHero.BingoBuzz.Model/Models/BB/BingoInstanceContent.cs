// <auto-generated> - Template:ModelsBackedByDto, Version:1.1, Id:f1539c0d-024f-4b1f-b346-132cdd9dd31f
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

	public class LoadRequestBingoInstanceContent : EventArgs
	{
		public LoadRequestBingoInstanceContent(string propertyNameRequestingLoad)
		{
			PropertyNameRequestingLoad = propertyNameRequestingLoad;
		}

		public string PropertyNameRequestingLoad { get; set; }
	}


	public partial class BingoInstanceContent : BaseModel<IWebApiDataServiceBB>, IBingoInstanceContent
	{
		public event EventHandler<LoadRequestBingoInstanceContent> OnLazyLoadRequest = delegate { }; // Empty delegate. Thus we are sure that value is always != null because no one outside of the class can change it.
		private xDTO.BingoInstanceContent _dto = null;

		public BingoInstanceContent(ILoggingService log, IDataService<IWebApiDataServiceBB> dataService) : base(log, dataService)
		{
			_dto = new xDTO.BingoInstanceContent();
			OnLazyLoadRequest += HandleLazyLoadRequest;
		}

		public BingoInstanceContent(ILoggingService log, IDataService<IWebApiDataServiceBB> dataService, xDTO.BingoInstanceContent dto) : this(log, dataService)
		{
			_dto = dto;
		}


		public virtual System.Guid BingoContentId { get { return _dto.BingoContentId; } }
		public virtual System.Guid BingoInstanceContentId { get { return _dto.BingoInstanceContentId; } }
		public virtual int BingoInstanceContentStatusTypeId { get { return _dto.BingoInstanceContentStatusTypeId; } }
		public virtual System.Guid BingoInstanceId { get { return _dto.BingoInstanceId; } }
		public virtual int Col { get { return _dto.Col; } }
		public virtual System.DateTime CreatedDate { get { return _dto.CreatedDate; } }
		public virtual System.Guid CreatedUserId { get { return _dto.CreatedUserId; } }
		public virtual bool FreeSquareIndicator { get { return _dto.FreeSquareIndicator; } }
		public virtual bool IsDeleted { get { return _dto.IsDeleted; } }
		public virtual int Row { get { return _dto.Row; } }
		public virtual System.DateTime UpdatedDate { get { return _dto.UpdatedDate; } }
		public virtual System.Guid UpdatedUserId { get { return _dto.UpdatedUserId; } }
		public virtual System.Guid UserId { get { return _dto.UserId; } }

		private IBingoContent _bingoContent = null; // Foreign Key
		private IBingoInstance _bingoInstance = null; // Foreign Key
		private IBingoInstanceContentStatusType _bingoInstanceContentStatusType = null; // Foreign Key
		private IUser _createdUser = null; // Foreign Key
		private IUser _updatedUser = null; // Foreign Key
		private IUser _user_UserId = null; // Foreign Key
		private List<IBingoInstanceEvent> _bingoInstanceEvents = null; // Reverse Navigation


		public virtual IBingoContent BingoContent
		{
			get
			{
				if (_bingoContent == null && _dto != null && _dto.BingoContent != null)
				{
					_bingoContent = new BingoContent(Log, DataService, _dto.BingoContent);
				}

				return _bingoContent;
			}
		}

		public virtual IBingoInstance BingoInstance
		{
			get
			{
				if (_bingoInstance == null)
				{
					OnLazyLoadRequest(this, new LoadRequestBingoInstanceContent(nameof(BingoInstance)));
				}

				return _bingoInstance;
			}
		}

		public virtual IBingoInstanceContentStatusType BingoInstanceContentStatusType
		{
			get
			{
				if (_bingoInstanceContentStatusType == null && _dto != null && _dto.BingoInstanceContentStatusType != null)
				{
					_bingoInstanceContentStatusType = new BingoInstanceContentStatusType(Log, DataService, _dto.BingoInstanceContentStatusType);
				}

				return _bingoInstanceContentStatusType;
			}
		}

		public virtual IUser CreatedUser
		{
			get
			{
				if (_createdUser == null && _dto != null && _dto.CreatedUser != null)
				{
					_createdUser = new User(Log, DataService, _dto.CreatedUser);
				}

				return _createdUser;
			}
		}

		public virtual IUser UpdatedUser
		{
			get
			{
				if (_updatedUser == null && _dto != null && _dto.UpdatedUser != null)
				{
					_updatedUser = new User(Log, DataService, _dto.UpdatedUser);
				}

				return _updatedUser;
			}
		}

		public virtual IUser User_UserId
		{
			get
			{
				if (_user_UserId == null && _dto != null && _dto.User_UserId != null)
				{
					_user_UserId = new User(Log, DataService, _dto.User_UserId);
				}

				return _user_UserId;
			}
		}

		public virtual List<IBingoInstanceEvent> BingoInstanceEvents
		{
			get
			{
				if (_bingoInstanceEvents == null && _dto != null)
				{	// The core DTO object is loaded, but this property is not loaded.
					if (_dto.BingoInstanceEvents != null)
					{	// The core DTO object has data for this property, load it into the model.
						_bingoInstanceEvents = new List<IBingoInstanceEvent>();
						foreach (var dtoItem in _dto.BingoInstanceEvents)
						{
							_bingoInstanceEvents.Add(new BingoInstanceEvent(Log, DataService, dtoItem));
						}
					}
					else
					{	// Trigger the load data request - The core DTO object is loaded and does not have data for this property.
						OnLazyLoadRequest(this, new LoadRequestBingoInstanceContent(nameof(BingoInstanceEvents)));
					}
				}

				return _bingoInstanceEvents;
			}
		}



	}
}
