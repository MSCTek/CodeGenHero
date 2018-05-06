using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGenHero.Logging;
using CodeGenHero.DataService;
using CodeGenHero.BingoBuzz.API.Client.Interface;
using CodeGenHero.BingoBuzz.DTO.BB;

namespace CodeGenHero.BingoBuzz.API.Client
{
	public partial class WebApiDataServiceBB : WebApiDataServiceBase, IWebApiDataServiceBB
	{

		public async Task<PageData<List<Meeting>>> GetMeetingsAndAttendeesByUserId(Guid userId, DateTime? minUpdatedDate, bool? isDeleted, string sort = null,
			int page = 1, int pageSize = 100)
		{
			List<IFilterCriterion> filterCriteria = new List<IFilterCriterion>();

			IFilterCriterion filterCriterion = new FilterCriterion();
			filterCriterion.FieldName = nameof(MeetingAttendee.UserId);
			filterCriterion.FieldType = "Guid";
			filterCriterion.FilterOperator = Constants.OPERATOR_ISEQUALTO;
			filterCriterion.Value = userId;
			filterCriteria.Add(filterCriterion);

			if (minUpdatedDate.HasValue)
			{
				filterCriterion = new FilterCriterion();
				filterCriterion.FieldName = nameof(Meeting.UpdatedDate);
				filterCriterion.FieldType = "DateTime?";
				filterCriterion.FilterOperator = Constants.OPERATOR_ISGREATERTHAN;
				filterCriterion.Value = minUpdatedDate;
				filterCriteria.Add(filterCriterion);
			}

			if (minUpdatedDate.HasValue)
			{
				filterCriterion = new FilterCriterion();
				filterCriterion.FieldName = nameof(Meeting.IsDeleted);
				filterCriterion.FieldType = "bool?";
				filterCriterion.FilterOperator = Constants.OPERATOR_ISEQUALTO;
				filterCriterion.Value = isDeleted;
				filterCriteria.Add(filterCriterion);
			}

			IPageDataRequest pageDataRequest = new PageDataRequest(filterCriteria: filterCriteria, sort: sort, page: page, pageSize: pageSize);
			List<string> filter = BuildFilter(pageDataRequest.FilterCriteria);
			return await SerializationHelper.Instance.SerializeCallResultsGet<List<Meeting>>(Log, GetClient(),
				$"{ExecutionContext.BaseWebApiUrl}GetMeetingsAndAttendeesByUserId", filter, page: pageDataRequest.Page, pageSize: pageDataRequest.PageSize);
		}


        public async Task<PageData<List<BingoInstance>>> GetInstancesAndEventsByMeetingId(Guid meetingId, DateTime? minUpdatedDate, bool? isDeleted, string sort = null, int page = 1, int pageSize = 100)
        {
            List<IFilterCriterion> filterCriteria = new List<IFilterCriterion>();

            IFilterCriterion filterCriterion = new FilterCriterion();
            filterCriterion.FieldName = nameof(BingoInstance.MeetingId);
            filterCriterion.FieldType = "Guid";
            filterCriterion.FilterOperator = Constants.OPERATOR_ISEQUALTO;
            filterCriterion.Value = meetingId;
            filterCriteria.Add(filterCriterion);

            if (minUpdatedDate.HasValue)
            {
                filterCriterion = new FilterCriterion();
                filterCriterion.FieldName = nameof(BingoInstance.UpdatedDate);
                filterCriterion.FieldType = "DateTime?";
                filterCriterion.FilterOperator = Constants.OPERATOR_ISGREATERTHAN;
                filterCriterion.Value = minUpdatedDate;
                filterCriteria.Add(filterCriterion);
            }

            if (minUpdatedDate.HasValue)
            {
                filterCriterion = new FilterCriterion();
                filterCriterion.FieldName = nameof(BingoInstance.IsDeleted);
                filterCriterion.FieldType = "bool?";
                filterCriterion.FilterOperator = Constants.OPERATOR_ISEQUALTO;
                filterCriterion.Value = isDeleted;
                filterCriteria.Add(filterCriterion);
            }

            IPageDataRequest pageDataRequest = new PageDataRequest(filterCriteria: filterCriteria, sort: sort, page: page, pageSize: pageSize);
            List<string> filter = BuildFilter(pageDataRequest.FilterCriteria);
            return await SerializationHelper.Instance.SerializeCallResultsGet<List<BingoInstance>>(Log, GetClient(),
                $"{ExecutionContext.BaseWebApiUrl}GetInstancesAndEventsByMeetingId", filter, page: pageDataRequest.Page, pageSize: pageDataRequest.PageSize);
        }

    }
}
