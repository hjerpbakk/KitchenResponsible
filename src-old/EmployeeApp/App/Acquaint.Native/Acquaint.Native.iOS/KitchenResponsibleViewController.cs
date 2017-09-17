using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Acquaint.Native.iOS
{
	[Register("KitchenResponsibleViewController")]
	public class KitchenResponsibleViewController : UITableViewController
	{
		public static readonly NSString WeekCellId;

		readonly KitchenResponsibleTableViewSource tableViewSource;

		static KitchenResponsibleViewController()
		{
			WeekCellId = new NSString("WeekCell");
		}

		public KitchenResponsibleViewController(IntPtr handle) : base(handle)
		{
			TableView.RegisterClassForCellReuse(typeof(WeekCell), WeekCellId);
			TableView.AllowsSelection = false;

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
