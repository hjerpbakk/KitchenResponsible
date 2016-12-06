using System;
using UIKit;

namespace Acquaint.Native.iOS
{
	public class TabBarViewController : UITabBarController
	{
		public TabBarViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
	}
}
