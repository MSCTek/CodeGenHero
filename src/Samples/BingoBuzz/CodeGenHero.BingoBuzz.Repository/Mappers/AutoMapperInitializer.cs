using AutoMapper;

namespace CodeGenHero.BingoBuzz.Repository.Mappers
{
	public class AutoMapperInitializer
	{
		public static void Initialize()
		{
			Mapper.Initialize(cfg =>
			{
				cfg.AddProfiles(typeof(AutoMapperInitializer));
			});
		}
	}
}