using AutoMapper;

namespace CodeGenHero.Repository.AutoMapper
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