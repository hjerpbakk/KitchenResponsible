using System;
using System.Threading.Tasks;
using Acquaint.Data;
using Acquaint.Models;
using Foundation;
using UIKit;

namespace Acquaint.Native.iOS
{
	public class KitchenResponsibleTableViewSource : UITableViewSource
	{
		private Week[] weeksWithResponsible;

		public KitchenResponsibleTableViewSource()
		{
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// TODO: create proper cell
			//var cell = tableView.DequeueReusableCell("WeekCell", indexPath) as UITableViewCell;
			var cell = new UITableViewCell(UITableViewCellStyle.Value1, "WeekCell");
			cell.TextLabel.Text = weeksWithResponsible[indexPath.Row].Responsible;
			cell.DetailTextLabel.Text = weeksWithResponsible[indexPath.Row].WeekNumber.ToString();
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section) =>
			weeksWithResponsible == null ? 0 : weeksWithResponsible.Length;

		public async Task LoadKitchenResponsibles()
		{
			// TODO: Service from container
			weeksWithResponsible = await (new KitchenResponsibleService()).Get();
		}
	}
}
