using System;
namespace AudioKetab
{
	public class MasterPageItem
	{
		public string Title { get; set; }
		public string IconSource { get; set; }
		public Type TargetType { get; set; }
        public Boolean IsVisible { get; set; }
	}
}
