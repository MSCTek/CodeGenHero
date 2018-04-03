using System;
using CodeGenHero.BingoBuzz.DTO.BB;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.SampleXam
{
    public static class DemoBingoContent
    {
        public static System.Guid SampleBingoContentId00 = Guid.Parse("886e09f3-947b-44cb-97cb-0ad1ce5dd7f1");
        public static System.Guid SampleBingoContentId01 = Guid.Parse("d37292f8-904c-45cf-9080-861caaec964f");

        public static BingoContent SampleBingoContent00
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId00,
                    Content = "SampleContent",
                    FreeSquareIndicator = false,
                    NumberOfUpvotes = 0,
                    NumberOfDownvotes = 0,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse("5ee57c47-7ce4-4833-ad73-126ee071da08"),
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = Guid.Parse("cc2218e4-708d-479a-9729-e4bb1f5f0f72"),
                    IsDeleted = false,
                };
            }
        }

        public static BingoContent SampleBingoContent01
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId01,
                    Content = "SampleContent",
                    FreeSquareIndicator = false,
                    NumberOfUpvotes = 0,
                    NumberOfDownvotes = 0,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse("13f812f0-1e7b-4823-8204-fcb59e01c85a"),
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = Guid.Parse("98f295c4-d7e6-46ae-bcaa-959388dc55b5"),
                    IsDeleted = false,
                };
            }
        }
    }
}