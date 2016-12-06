using System;
using Foundation;
using UIKit;

namespace Acquaint.Native.iOS
{
	[Register("TabBarViewController")]
	public class TabBarViewController : UITabBarController
	{
		public TabBarViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
	}
}
