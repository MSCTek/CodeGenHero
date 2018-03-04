using CodeGenHero.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoBingoContent
    {
        public static Guid SampleBingoContentId01 = Guid.Parse("4bbed083-7610-4aad-9945-86a6445294a0");
        public static Guid SampleBingoContentId02 = Guid.Parse("54cee266-2a44-4d97-97e6-921ec0718789");
        public static Guid SampleBingoContentId03 = Guid.Parse("b4441e66-1149-46c2-a670-1be7001faba9");
        public static Guid SampleBingoContentId04 = Guid.Parse("a7edf02f-866c-4323-9cc4-787b429b4515");
        public static Guid SampleBingoContentId05 = Guid.Parse("a27968b6-f554-480c-a3dc-62335a41620b");
        public static Guid SampleBingoContentId06 = Guid.Parse("6b8f7791-cf83-44b3-8549-f98be90dbb3f");
        public static Guid SampleBingoContentId07 = Guid.Parse("be446b0d-eb3a-4fe5-a52a-b7a2dc6acd6d");
        public static Guid SampleBingoContentId08 = Guid.Parse("81949e35-144e-4681-aa6e-0a63ea9b2a83");
        public static Guid SampleBingoContentId09 = Guid.Parse("fc9f7067-a6d4-4997-96f8-37daf3d93b65");
        public static Guid SampleBingoContentId10 = Guid.Parse("6e2806fe-2218-44b4-a443-92a8783dd8da");
        public static Guid SampleBingoContentId11 = Guid.Parse("d90e7432-48bf-48b0-805a-455ed6c7628b");
        public static Guid SampleBingoContentId12 = Guid.Parse("3b5aae51-9aad-4b10-adf3-10b9f68165c5");
        public static Guid SampleBingoContentId13 = Guid.Parse("bb42ab1f-e0d3-45bc-b98e-497496e21b11");
        public static Guid SampleBingoContentId14 = Guid.Parse("e2008855-b5f8-416a-ad44-f4cb61f25ed4");
        public static Guid SampleBingoContentId15 = Guid.Parse("82bee588-96f7-41f2-b36e-6e27ef9d1696");
        public static Guid SampleBingoContentId16 = Guid.Parse("d9bd1e85-a085-4c62-8e4c-c70c483162f6");
        public static Guid SampleBingoContentId17 = Guid.Parse("a4f19bec-752d-4623-8a1a-c807d446b299");
        public static Guid SampleBingoContentId18 = Guid.Parse("bf3b16d0-3008-4542-8861-e110b2efe847");
        public static Guid SampleBingoContentId19 = Guid.Parse("f0028b07-f597-4cc7-a7a7-b470dfc3efbf");
        public static Guid SampleBingoContentId20 = Guid.Parse("9495185e-448c-457a-96a4-e3dd3a99c621");
        public static Guid SampleBingoContentId21 = Guid.Parse("0455b450-58c6-4b9a-b154-a100a487af1c");
        public static Guid SampleBingoContentId22 = Guid.Parse("25eb5592-755d-4625-9b07-256fb8474403");
        public static Guid SampleBingoContentId23 = Guid.Parse("c3974c5d-617a-4fd5-9e27-bf169226d4d1");
        public static Guid SampleBingoContentId24 = Guid.Parse("6d97fa8f-e5c1-4809-93b8-192a13e70f5d");
        public static Guid SampleBingoContentId25 = Guid.Parse("ac95b0de-4983-4627-8950-99a72827bc98");

        public static BingoContent SampleBingoContent01
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId01,
                    Content = "'Let's take this offline'",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent02
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId02,
                    Content = "'Keep me in the loop'",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent03
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId03,
                    Content = "Sports comment",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent04
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId04,
                    Content = "Discussion of requirements modification",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent05
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId05,
                    Content = "Boss makes an unfunny joke but everyone laughs",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent06
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId06,
                    Content = "'Shoot me an email'",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent07
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId07,
                    Content = "Technical difficulties",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent08
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId08,
                    Content = "Background cellphone ring or ding",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent09
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId09,
                    Content = "Painful silence",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent10
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId10,
                    Content = "Conference call muted to side-talk",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent11
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId11,
                    Content = "Day of the week used as excuse for behavior",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent12
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId12,
                    Content = "Weather comment",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent13
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId13,
                    Content = "FREE SQUARE",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = true,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent14
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId14,
                    Content = "Someone broke the build",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent15
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId15,
                    Content = "'Hi, who just joined?'",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent16
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId16,
                    Content = "Spoiler alert for TV, Film or Video Game",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent17
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId17,
                    Content = "Mysterious botched code commit or merge",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent18
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId18,
                    Content = "Accidental screen sharing",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent19
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId19,
                    Content = "Mysterious (child, animal or otherwise) noises",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent20
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId20,
                    Content = "Re-bug of past defect",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent21
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId21,
                    Content = "Someone talking on mute",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent22
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId22,
                    Content = "Sound of someone dozing off",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent23
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId23,
                    Content = "Coughing or throat clearing",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent24
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId24,
                    Content = "Go-No-Go",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }

        public static BingoContent SampleBingoContent25
        {
            get
            {
                return new BingoContent()
                {
                    BingoContentId = SampleBingoContentId25,
                    Content = "Sound of food or drink being consumed",
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    FreeSquareIndicator = false,
                    IsDeleted = false,
                    NumberOfDownvotes = 0,
                    NumberOfUpvotes = 0,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = DemoUser.UserIdAlexander
                };
            }
        }
    }
}