using System;
using System.Threading.Tasks;
using Acquaint.Data;
using Acquaint.Models;
using Acquaint.Util;
using FFImageLoading;
using FFImageLoading.Transformations;
using Foundation;
using UIKit;

namespace Acquaint.Native.iOS
{
	public class KitchenResponsibleTableViewSource : UITableViewSource
	{
		Week[] weeks;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (WeekCell)tableView.DequeueReusableCell(KitchenResponsibleViewController.WeekCellId, indexPath);
			cell.TextLabel.Text = weeks[indexPath.Section].Responsible;
			try
			{
				ImageService
				.Instance
				.LoadUrl(/*weeks[indexPath.Section].SmallPhotoUrl*/"https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50", TimeSpan.FromHours(Settings.ImageCacheDurationHours))  // get the image from a URL
				.LoadingPlaceholder("placeholderProfileImage.png")                                          // specify a placeholder image
				.Transform(new CircleTransformation())                                                      // transform the image to a circle
				.Error(e => System.Diagnostics.Debug.WriteLine(e.Message))
				.Into(cell.ImageView);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section) =>
			weeks.Length == 0 ? 0 : 1;

		public override nint NumberOfSections(UITableView tableView) =>
			weeks == null ? 0 : weeks.Length;

		public override string TitleForHeader(UITableView tableView, nint section) =>
			weeks[section].WeekNumber.ToString();

		public async Task LoadKitchenResponsibles()
		{
			// TODO: Service from container
			weeks = await (new KitchenResponsibleService()).Get();
		}
	}
}
