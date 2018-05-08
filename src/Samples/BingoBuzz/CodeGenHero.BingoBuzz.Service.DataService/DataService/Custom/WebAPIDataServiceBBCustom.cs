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

		public async Task<PageData<List<Meeting>>> GetMeetingsAndAttendeesByUserId(Guid userId, DateTime? minUpdatedDate, bool? isDeleted, string sort = null, int page = 1, int pageSize = 100)
		{
			List<IFilterCriterion> filterCriteria = new List<IFilterCriterion>();

			IFilterCriterion filterCriterion = new FilterCriterion
			{
				FieldName = nameof(MeetingAttendee.UserId),
				FieldType = "Guid",
				FilterOperator = Constants.OPERATOR_ISEQUALTO,
				Value = userId
			};
			filterCriteria.Add(filterCriterion);

			if (minUpdatedDate.HasValue)
			{
				filterCriterion = new FilterCriterion
				{
					FieldName = nameof(Meeting.UpdatedDate),
					FieldType = "DateTime?",
					FilterOperator = Constants.OPERATOR_ISGREATERTHAN,
					Value = minUpdatedDate
				};
				filterCriteria.Add(filterCriterion);
			}

			if (minUpdatedDate.HasValue)
			{
				filterCriterion = new FilterCriterion
				{
					FieldName = nameof(Meeting.IsDeleted),
					FieldType = "bool?",
					FilterOperator = Constants.OPERATOR_ISEQUALTO,
					Value = isDeleted
				};
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

			IFilterCriterion filterCriterion = new FilterCriterion
			{
				FieldName = nameof(BingoInstance.MeetingId),
				FieldType = "Guid",
				FilterOperator = Constants.OPERATOR_ISEQUALTO,
				Value = meetingId
			};
			filterCriteria.Add(filterCriterion);

            if (minUpdatedDate.HasValue)
            {
				filterCriterion = new FilterCriterion
				{
					FieldName = nameof(BingoInstance.UpdatedDate),
					FieldType = "DateTime?",
					FilterOperator = Constants.OPERATOR_ISGREATERTHAN,
					Value = minUpdatedDate
				};
				filterCriteria.Add(filterCriterion);
            }

            if (minUpdatedDate.HasValue)
            {
				filterCriterion = new FilterCriterion
				{
					FieldName = nameof(BingoInstance.IsDeleted),
					FieldType = "bool?",
					FilterOperator = Constants.OPERATOR_ISEQUALTO,
					Value = isDeleted
				};
				filterCriteria.Add(filterCriterion);
            }

            IPageDataRequest pageDataRequest = new PageDataRequest(filterCriteria: filterCriteria, sort: sort, page: page, pageSize: pageSize);
            List<string> filter = BuildFilter(pageDataRequest.FilterCriteria);
            return await SerializationHelper.Instance.SerializeCallResultsGet<List<BingoInstance>>(Log, GetClient(),
                $"{ExecutionContext.BaseWebApiUrl}GetInstancesAndEventsByMeetingId", filter, page: pageDataRequest.Page, pageSize: pageDataRequest.PageSize);
        }


        public async Task<HttpCallResult<BingoInstance>> CreateBingoInstanceWithContentAsync(BingoInstance item)
        {
            var retVal = await SerializationHelper.Instance.SerializeCallResultsPost<BingoInstance>(
                    Log, GetClient(),
                    $"{ExecutionContext.BaseWebApiUrl}BingoInstances/", item);
            return retVal;
        }

    }
}
