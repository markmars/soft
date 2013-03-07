using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MarkMars.UI.TrueLorePanel
{
    public interface IPanel
    {
        PanelStyle PanelStyle { get;set;}
		ColorScheme ColorScheme { get;set;}
        Image Image { get;set;}
		int CaptionHeight { get;}
        Font CaptionFont { get;set;}
        Color ColorCaptionGradientBegin { get;set;}
        Color ColorCaptionGradientMiddle { get;set;}
        Color ColorCaptionGradientEnd { get;set;}
        bool ShowBorder { get;set;}
    }
}
