// <auto-generated> - Template:MvvmLightModelObject, Version:1.0, Id:4d03f2c7-de26-4aee-a267-cad747c9606a
using GalaSoft.MvvmLight;

namespace CodeGenHero.BingoBuzz.Xam.ModelObj.BB
{
	public partial class BingoInstanceEventType : ObservableObject
	{
		public BingoInstanceEventType()
		{
			BingoInstanceEvents = new System.Collections.Generic.List<BingoInstanceEvent>(); // Reverse Navigation

			InitializePartial();
		}

		private int _bingoInstanceEventTypeId;
		private string _name;


		public int BingoInstanceEventTypeId
		{
			get { return _bingoInstanceEventTypeId; }
			set
			{
				Set<int>(() => BingoInstanceEventTypeId, ref _bingoInstanceEventTypeId, value);
				RunCustomLogicSetBingoInstanceEventTypeId(value);
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				Set<string>(() => Name, ref _name, value);
				RunCustomLogicSetName(value);
			}
		}

		public virtual System.Collections.Generic.IList<BingoInstanceEvent> BingoInstanceEvents { get; set; } // Many to many mapping


		partial void InitializePartial();

		#region RunCustomLogicSet

		partial void RunCustomLogicSetBingoInstanceEventTypeId(int value);
		partial void RunCustomLogicSetName(string value);

		#endregion RunCustomLogicSet

	}
}