// This file has been autogenerated from a class added in the UI designer.

using System;
using Acquaint.Util;
using FFImageLoading;
using FFImageLoading.Cache;
using Foundation;
using UIKit;

namespace Acquaint.Native.iOS
{
	public partial class SettingsViewController : UITableViewController
	{
		UIBarButtonItem _SaveBarButtonItem;
		UIBarButtonItem _CancelBarButtonItem;

		readonly UIColor InvalidEntryColor = UIColor.Red;

		public SettingsViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_SaveBarButtonItem = NavigationItem.RightBarButtonItems[0];

			_CancelBarButtonItem = NavigationItem.RightBarButtonItems[1];

			_SaveBarButtonItem.Clicked += HandleSaveBarButtonItemClicked;

			_CancelBarButtonItem.Clicked += HandleCancelBarButtonItemClicked;

			ResetToDefaultsSwitch.ValueChanged += (sender, e) => 
			{
				if ((sender as UISwitch).On)
				{
					ClearImageCacheSwitch.On = true;
				}
			};

			LoadValuesForFormFields();
		}

		void LoadValuesForFormFields()
		{
			DataPartitionPhraseEntry.Text = Settings.DataPartitionPhrase;

			BackendUrlEntry.Text = Settings.AzureAppServiceUrl;

			ImageCacheDurationEntry.Text = Settings.ImageCacheDurationHours.ToString();

			ClearImageCacheSwitch.On = false;

			ResetToDefaultsSwitch.On = false;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			TableView.BackgroundView = new UIView(TableView.Bounds) { BackgroundColor = UIColor.White };
		}

		async void HandleSaveBarButtonItemClicked(object sender, EventArgs ea)
		{
			// if the reset to defaults has been requested
			if (ResetToDefaultsSwitch.On)
			{
				// reset the settings
				Settings.ResetUserConfigurableSettingsToDefaults();

				// set image cache clear flag to true
				Settings.ClearImageCacheIsRequested = true;
			}
			// if image cache clear has been explicitly requested
			else if (ClearImageCacheSwitch.On)
			{
				// set image cache clear flag to true
				Settings.ClearImageCacheIsRequested = true;
			}
			else
			{
				// Check the data partition id
				if (string.IsNullOrWhiteSpace(DataPartitionPhraseEntry.Text))
				{
					// activate validation indicator for DataPartitionPhraseEntry
					DataPartitionPhraseEntry.AttributedPlaceholder =
					new NSAttributedString(
						DataPartitionPhraseEntry.Placeholder,
						UIFont.SystemFontOfSize(UIFont.SystemFontSize),
						InvalidEntryColor);

					return;
				}

				// Check the backend service URL
				Uri testUri;
				if (!Uri.TryCreate(BackendUrlEntry.Text, UriKind.Absolute, out testUri))
				{
					//pop an alert to indicate the URL is invalid
					using (var alert =
					       new UIAlertView(
							"Invalid URL",
							"Please enter a valid URL",
							null,
							"OK"))
					{
						alert.Show();
					}

					return;
				}

				int localStoreResetConditions = 0;

				// if the backend service URL has changed, then we want to reset the local datastore
				if (Settings.AzureAppServiceUrl.ToLower() != BackendUrlEntry.Text.ToLower())
					localStoreResetConditions++;

				// set the backend service URL
				Settings.AzureAppServiceUrl = BackendUrlEntry.Text;

				// if the data partition phrase has changed, then we want to reset the local datastore
				if (Settings.DataPartitionPhrase.ToLower() != DataPartitionPhraseEntry.Text.ToLower())
					localStoreResetConditions++;

				// set the data partition phrase
				Settings.DataPartitionPhrase = DataPartitionPhraseEntry.Text;

				// if we've triggered at last one condition for local datastore, then set the flag in the Settings class
				if (localStoreResetConditions > 0)
					Settings.LocalDataResetIsRequested = true;

				// we're enforcing a minimum image cache duration
				int ImageCacheDurationValue = Settings.ImageCacheDurationHoursDefault;

				int.TryParse(ImageCacheDurationEntry.Text, out ImageCacheDurationValue);

				if (ImageCacheDurationValue < Settings.ImageCacheDurationHoursDefault)
					ImageCacheDurationEntry.Text = Settings.ImageCacheDurationHoursDefault.ToString();

				// if either the image cache sureation has changed or local datastore reset is being requested, then clear the image cache
				if (Settings.ImageCacheDurationHours != ImageCacheDurationValue || Settings.LocalDataResetIsRequested)
					Settings.ClearImageCacheIsRequested = true;

				// set the image cache duration
				Settings.ImageCacheDurationHours = ImageCacheDurationValue;
			}

			// if image cache reset has been requested
			if (Settings.ClearImageCacheIsRequested)
			{
				// clear image cache
				await ImageService
					.Instance
					.InvalidateCacheAsync(CacheType.All);
			}

			// dismiss the view controller
			DismissViewController(true, null);
		}

		void HandleCancelBarButtonItemClicked(object sender, EventArgs ea)
		{
			DismissViewController(true, null);
		}
	}
}
