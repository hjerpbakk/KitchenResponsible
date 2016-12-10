using System;
using System.Threading.Tasks;
using Acquaint.Data;
using Foundation;
using UIKit;

namespace Acquaint.Native.iOS
{
	public class KitchenResponsibleTableViewSource : UITableViewSource
	{
		string[] titles;
		string[] cellContent;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (WeekCell)tableView.DequeueReusableCell(KitchenResponsibleViewController.WeekCellId, indexPath);
			cell.TextLabel.Text = cellContent[indexPath.Section];
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section) =>
			cellContent.Length == 0 ? 0 : 1;

		public override nint NumberOfSections(UITableView tableView) =>
			cellContent == null ? 0 : cellContent.Length;

		public override string TitleForHeader(UITableView tableView, nint section) =>
			titles[section];

		public async Task LoadKitchenResponsibles()
		{
			// TODO: Service from container
			var weeksWithResponsible = await (new KitchenResponsibleService()).Get();
			titles = new string[weeksWithResponsible.Length];
			cellContent = new string[weeksWithResponsible.Length];
			for (int i = 0; i < weeksWithResponsible.Length; i++)
			{
				titles[i] = weeksWithResponsible[i].WeekNumber.ToString();
				cellContent[i] = weeksWithResponsible[i].Responsible;
			}
		}
	}
}
