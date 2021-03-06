using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.MessagePage
{
    public partial class MessagePageCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MessagePageCell");
        public static readonly UINib Nib;

        static MessagePageCell()
        {
            Nib = UINib.FromName("MessagePageCell", NSBundle.MainBundle);
        }

        protected MessagePageCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Update(MessageItemViewModel message)
        {
            StyleUtil.InitLabelWithSpacing(Label1, StyleUtil.FontType.FontRegular, message.Title, 1.14, 16, 28);
            StyleUtil.InitLabelWithSpacing(Label2, StyleUtil.FontType.FontRegular,
                DateUtils.GetDateFromDateTime(message.TimeStamp, "d. MMMMM"), 1.14, 12, 17);
            StyleUtil.InitLabelWithSpacing(Label3, StyleUtil.FontType.FontRegular,
                MessageItemViewModel.MESSAGES_RECOMMENDATIONS, 1.14, 12, 17);
            IndicatorView.Alpha = message.IsRead ? 0 : 1;
            BackgroundColor = message.IsRead
                ? new UIColor(new nfloat(1), new nfloat(0.1))
                : new UIColor(new nfloat(1), new nfloat(0.25));
            IndicatorView.Layer.CornerRadius = IndicatorView.Layer.Frame.Height / 2;
            if (AppDelegate.ShouldOperateIn12_5Mode)
            {
                Chevron.Image = UIImage.FromBundle("ChevronRight");
                Chevron.ContentMode = UIViewContentMode.ScaleAspectFit;
            }
        }
    }
}