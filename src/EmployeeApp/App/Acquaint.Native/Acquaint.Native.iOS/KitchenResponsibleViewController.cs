using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Acquaint.Native.iOS
{
	[Register("KitchenResponsibleViewController")]
	public class KitchenResponsibleViewController : UITableViewController
	{
		readonly KitchenResponsibleTableViewSource tableViewSource;

		public KitchenResponsibleViewController(IntPtr handle) : base(handle)
		{
			tableViewSource = new KitchenResponsibleTableViewSource();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.Source = tableViewSource;
		}

		public override async void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			await RefreshKitchenResponsibles();
		}

		async Task RefreshKitchenResponsibles()
		{
			try
			{
				await tableViewSource.LoadKitchenResponsibles();
				TableView.ReloadData();
			}
			catch (Exception ex)
			{

			}
		}
	}
}
