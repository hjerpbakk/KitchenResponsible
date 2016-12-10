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
		string[] titles;
		string[] cellContent;

		public KitchenResponsibleTableViewSource()
		{
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// TODO: create proper cell
			//var cell = tableView.DequeueReusableCell("WeekCell", indexPath) as UITableViewCell;
			var cell = new UITableViewCell(UITableViewCellStyle.Value1, "WeekCell");
			cell.TextLabel.Text = cellContent[indexPath.Section];
			//cell.DetailTextLabel.Text = weeksWithResponsible[indexPath.Row].WeekNumber.ToString();
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section) =>
			cellContent.Length == 0 ? 0 : 1;

		public override nint NumberOfSections(UITableView tableView)
		{
			return cellContent == null ? 0 : cellContent.Length;
		}

		//public override string[] SectionIndexTitles(UITableView tableView)
		//{
		//	return titles;
		//}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return titles[section];
		}

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
