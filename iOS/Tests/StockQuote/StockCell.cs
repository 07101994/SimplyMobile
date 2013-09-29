using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Stock = SimplyMobile.Plugins.WcfStockService.StockQuote;

namespace StockQuote
{
	public partial class StockCell : UITableViewCell
	{
		private Stock stock;

		public static readonly NSString Key = new NSString ("StockCell");
		public static readonly UINib Nib;

		public void Bind(Stock stock)
		{
			this.labelStockName.Text = stock.Name;
			this.labelHighLow.Text = string.Format ("{0}/{1}", stock.High, stock.Low);
			this.labelLast.Text = stock.Last;

			if (this.stock == null)
			{
				this.switchMonitor.ValueChanged += async (sender, e) => 
				{
					if (this.switchMonitor.On)
					{
						this.switchMonitor.Enabled = false;
						this.activityUpdating.StartAnimating();

						try
						{
							var s = await AppDelegate.StockClient.GetStockQuoteAsync(this.stock.Symbol);
							this.labelLast.Text = s.Last;
						}
						catch{}

						this.activityUpdating.StopAnimating();
						this.switchMonitor.On = false;
						this.switchMonitor.Enabled = true;
					}
				};
			}

			this.stock = stock;
		}

		static StockCell ()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				Nib = UINib.FromName ("StockCell_iPhone", NSBundle.MainBundle);
			else
				Nib = UINib.FromName ("StockCell_iPad", NSBundle.MainBundle);
		}

		public StockCell (IntPtr handle) : base (handle)
		{

		}

		public static StockCell Create ()
		{
			return (StockCell)Nib.Instantiate (null, null) [0];
		}
	}
}

