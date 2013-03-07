using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MarkMars.UI.TrueLorePanel
{
	#region Class Panel
	/// <summary>
	/// Used to group collections of controls. 
	/// </summary>
	/// <remarks>
	/// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
	/// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
	/// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
	/// PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
	/// REMAINS UNCHANGED.
	/// </remarks>
	/// <copyright>Copyright ?2006-2007 Uwe Eichkorn</copyright>
	[Designer(typeof(PanelDesigner)),
	DesignTimeVisibleAttribute(true)]
	[ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
	public partial class Panel : BasePanel
    {
        #region FieldsPrivate

        private bool m_bShowTransparentBackground;
		private bool m_bShowXPanderPanelProfessionalStyle;
        private LinearGradientMode m_linearGradientMode;
		private PanelStyle m_ePanelStyle;
        private System.Drawing.Color m_colorContentPanelGradientBegin = ProfessionalColors.ToolStripContentPanelGradientBegin;
        private System.Drawing.Color m_colorContentPanelGradientEnd = ProfessionalColors.ToolStripContentPanelGradientEnd;
		
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the style of the Panel
		/// </summary>
		[Description("Style of the Panel")]
        [DefaultValue(0)]
		[Category("Appearance")]
		public override PanelStyle PanelStyle
		{
            get { return this.m_ePanelStyle; }
			set
			{
                if (value != this.m_ePanelStyle)
                {
                    this.m_ePanelStyle = value;
                    switch (this.m_ePanelStyle)
                    {
                        case PanelStyle.Aqua:
                            this.DockPadding.Top = this.CaptionHeight + CaptionSpacing;
                            this.DockPadding.Left = CaptionSpacing;
                            this.DockPadding.Right = CaptionSpacing;
                            this.DockPadding.Bottom = CaptionSpacing;
                            break;
                        case PanelStyle.Default:
                            this.DockPadding.Top = this.CaptionHeight;
                            this.DockPadding.Left = 0;
                            this.DockPadding.Right = 0;
                            this.DockPadding.Bottom = 0;
                            break;
                        case PanelStyle.None:
                            this.DockPadding.Top = 0;
                            this.DockPadding.Left = 0;
                            this.DockPadding.Right = 0;
                            this.DockPadding.Bottom = 0;
                            break;
                    }
                    this.Invalidate();
                }
			}
		}
		/// <summary>
		/// Gets or sets the starting color of the gradient used in the panels background
		/// </summary>
		[Description("Gets or sets the starting color of the gradient used in the panels background")]
		[Category("Appearance")]
		public Color ColorContentPanelGradientBegin
		{
			get { return this.m_colorContentPanelGradientBegin; }
			set
			{
                if (value != this.m_colorContentPanelGradientBegin)
                {
                    this.m_colorContentPanelGradientBegin = value;
                    this.Invalidate();
                }
			}
		}
		/// <summary>
		/// Gets or sets the end color of the gradient used in the panels background
		/// </summary>
		[Description("Gets or sets the end color of the gradient used in the panels background")]
		[Category("Appearance")]
		public Color ColorContentPanelGradientEnd
		{
			get { return this.m_colorContentPanelGradientEnd; }
			set
			{
                if (value != this.m_colorContentPanelGradientEnd)
                {
                    this.m_colorContentPanelGradientEnd = value;
                    this.Invalidate();
                }
			}
		}
		/// <summary>
		/// LinearGradientMode of the panels background
		/// </summary>
        [Description("LinearGradientMode of the Panels background"),
        DefaultValue(1),
        Category("Appearance")]
        public LinearGradientMode LinearGradientMode
        {
            get { return this.m_linearGradientMode; }
            set
            {
                if (value != this.m_linearGradientMode)
                {
                    this.m_linearGradientMode = value;
                    this.Invalidate();
                }
            }
        }
		/// <summary>
		/// Gets or sets a value indicating whether the controls background is transparent
		/// </summary>
		[Description("Gets or sets a value indicating whether the controls background is transparent")]
        [DefaultValue(false)]
        [Category("Behavior")]
		public bool ShowTransparentBackground
		{
			get { return this.m_bShowTransparentBackground; }
			set
			{
                if (value != this.m_bShowTransparentBackground)
                {
                    this.m_bShowTransparentBackground = value;
                    this.Invalidate();
                }
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether the controls caption professional colorscheme is the same then the XPanderPanels
		/// </summary>
		[Description("Gets or sets a value indicating whether the controls caption professional colorscheme is the same then the XPanderPanels")]
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool ShowXPanderPanelProfessionalStyle
		{
			get { return this.m_bShowXPanderPanelProfessionalStyle; }
			set
			{
                if (value != this.m_bShowXPanderPanelProfessionalStyle)
                {
                    this.m_bShowXPanderPanelProfessionalStyle = value;
                    this.Invalidate();
                }
			}
		}

		#endregion

		#region MethodsPublic
		/// <summary>
		/// Initializes a new instance of the Panel class.
		/// </summary>
		public Panel()
		{
			InitializeComponent();

            this.CaptionFont = new Font("Arial", SystemFonts.MenuFont.SizeInPoints + 1.5F, FontStyle.Bold);
            this.CaptionForeColor = SystemColors.ActiveCaptionText;
            this.BackColor = SystemColors.Window;
			this.ForeColor = SystemColors.ControlText;
			this.ShowTransparentBackground = false;
			this.ShowXPanderPanelProfessionalStyle = false;
			this.ColorScheme = ColorScheme.Professional;
			this.DockPadding.Top = this.CaptionHeight;
            this.LinearGradientMode = LinearGradientMode.Vertical;
		}

		#endregion

		#region MethodsProtected
		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
			if (this.ShowTransparentBackground == true)
			{
				this.BackColor = Color.Transparent;
			}
			else
			{
				Color colorContentPanelGradientBegin = ProfessionalColors.ToolStripContentPanelGradientEnd;
				Color colorContentPanelGradientEnd = ProfessionalColors.ToolStripContentPanelGradientBegin;

				if (this.ColorScheme == ColorScheme.Custom)
				{
					colorContentPanelGradientBegin = this.m_colorContentPanelGradientBegin;
					colorContentPanelGradientEnd = this.m_colorContentPanelGradientEnd;
				}

				Rectangle rectangleBounds = new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
				RenderBackgroundGradient(
					pevent.Graphics,
					rectangleBounds,
					colorContentPanelGradientBegin,
					colorContentPanelGradientEnd,
					this.LinearGradientMode);
			}
		}
		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
            switch (this.m_ePanelStyle)
            {
                case PanelStyle.Default:
                    DrawStyleDefault(e);
                    break;
                case PanelStyle.Aqua:
                    DrawStyleAqua(e);
                    break;
            }
		}

		#endregion

		#region MethodsPrivate

		private void DrawStyleDefault(PaintEventArgs e)
		{
            Rectangle rectangle = this.CaptionRectangle;
            
            Color colorGradientBegin = ProfessionalColors.OverflowButtonGradientMiddle;
            Color colorGradientEnd = ProfessionalColors.OverflowButtonGradientEnd;

			if (this.ColorScheme == ColorScheme.Custom)
            {
                colorGradientBegin = this.ColorCaptionGradientBegin;
                colorGradientEnd = this.ColorCaptionGradientEnd;
            }

			if (this.ShowXPanderPanelProfessionalStyle == true)
			{
				RenderDoubleBackgroundGradient(
					e.Graphics,
					rectangle,
					ProfessionalColors.ToolStripGradientBegin,
					ProfessionalColors.ToolStripGradientMiddle,
					ProfessionalColors.ToolStripGradientEnd,
					LinearGradientMode.Vertical,
					true);
			}
			else
			{
				RenderBackgroundGradient(
					e.Graphics,
					rectangle,
					colorGradientBegin,
					colorGradientEnd,
					LinearGradientMode.Vertical);
			}

            int iTextPositionX = CaptionSpacing;
            if (this.Image != null)
            {
                Rectangle imageRectangle = this.ImageRectangle;
				if (this.RightToLeft == RightToLeft.No)
                {
					DrawImage(e.Graphics, this.Image, imageRectangle);
                    iTextPositionX += this.ImageSize.Width + CaptionSpacing;
                }
                else
                {
                    imageRectangle.X = this.ClientRectangle.Right - RightImagePositionX;
                    DrawImage(e.Graphics, this.Image, imageRectangle);
                }
            }
            Rectangle textRectangle = rectangle;
            textRectangle.X = iTextPositionX;
            textRectangle.Width -= iTextPositionX + CaptionSpacing;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                if (this.Image != null)
                {
                    textRectangle.Width -= RightImagePositionX;
                }
            }
            
            //ªÊ÷∆±≥æ∞Õº∆¨
            if (this.BackgroundImage != null)
            {
                e.Graphics.DrawImage(this.BackgroundImage, new Rectangle(this.Left, this.Top, this.Width + 5, this.Height));
            }

            DrawString(e.Graphics, textRectangle, this.CaptionFont, this.CaptionForeColor, this.Text, this.RightToLeft);
		}

        private void DrawStyleAqua(PaintEventArgs e)
        {
            Color colorGradientBegin = ProfessionalColors.OverflowButtonGradientBegin;
            Color colorGradientEnd = ProfessionalColors.MenuBorder;

            if (this.ColorScheme == ColorScheme.Custom)
            {
                colorGradientBegin = this.ColorCaptionGradientBegin;
                colorGradientEnd = this.ColorCaptionGradientEnd;
            }
            if (this.ShowXPanderPanelProfessionalStyle == true)
            {
                colorGradientEnd = ProfessionalColors.ToolStripGradientEnd;
                colorGradientBegin = ProfessionalColors.ToolStripGradientBegin;
            }
            ///
            /// Outer Caption Rectangle
            ///
            Rectangle outerRectangle = new Rectangle(
                CaptionSpacing,
                CaptionSpacing,
                this.ClientRectangle.Width - (2 * CaptionSpacing),
                this.CaptionHeight);

			using (GraphicsPath outerGraphicsPath = GetBackgroundPath(outerRectangle, 20))
			{
				using (LinearGradientBrush outerLinearGradientBrush = new LinearGradientBrush(
					outerRectangle,
					colorGradientBegin,
					colorGradientEnd,
					LinearGradientMode.Vertical))
				{
					e.Graphics.FillPath(outerLinearGradientBrush, outerGraphicsPath); //draw top bubble
				}
			}
            ///
            /// Create top water color to give "aqua" effect
            /// 
            Rectangle innerRectangle = outerRectangle;
            innerRectangle.Height = 14;

			using (GraphicsPath innerGraphicsPath = GetPath(innerRectangle, 20))
			{
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(
					innerRectangle,
					Color.FromArgb(255, Color.White),
					Color.FromArgb(32, Color.White),
					LinearGradientMode.Vertical))
				{
					///
					///draw shapes
					///
					e.Graphics.FillPath(linearGradientBrush, innerGraphicsPath); //draw top bubble
				}
			}

            int iTextPositionX = outerRectangle.Left + CaptionSpacing;
			if (this.Image != null)
			{
				Rectangle imageRectangle = this.ImageRectangle;
				imageRectangle.Y = outerRectangle.Top + ImagePaddingTop;
				if (this.RightToLeft == RightToLeft.No)
				{
					imageRectangle.X = outerRectangle.Left + CaptionSpacing;
					iTextPositionX += this.ImageSize.Width + CaptionSpacing;
				}
				else
				{
					imageRectangle.X = outerRectangle.Right - RightImagePositionX;
				}
				DrawImage(e.Graphics, this.Image, imageRectangle);
			}
			Rectangle textRectangle = outerRectangle;
			textRectangle.X = iTextPositionX;
			textRectangle.Width -= iTextPositionX + CaptionSpacing;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				textRectangle.Width += CaptionSpacing;
				if (this.Image != null)
				{
					textRectangle.Width -= RightImagePositionX;
				}
			}
			DrawString(e.Graphics, textRectangle, this.CaptionFont, this.CaptionForeColor, this.Text, this.RightToLeft);
        }

		#endregion

	}

	#endregion

	#region Class PanelDesigner

	internal class PanelDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		#region MethodsPublic

		public PanelDesigner()
		{
		}

		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize(component);
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				// Create action list collection
				DesignerActionListCollection actionLists = new DesignerActionListCollection();

				// Add custom action list
				actionLists.Add(new PanelDesignerActionList(this.Component));

				// Return to the designer action service
				return actionLists;
			}
		}

		#endregion

		#region MethodsProtected

		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			base.OnPaintAdornments(e);
		}

		#endregion
	}

	#endregion

	#region Class XPanderPanelListDesignerActionList

	public class PanelDesignerActionList : DesignerActionList
	{
		#region Properties

		public bool ShowTransparentBackground
		{
			get { return this.Panel.ShowTransparentBackground; }
			set { SetProperty("ShowTransparentBackground", value); }
		}
		public bool ShowXPanderPanelProfessionalStyle
		{
			get { return this.Panel.ShowXPanderPanelProfessionalStyle; }
			set { SetProperty("ShowXPanderPanelProfessionalStyle", value); }
		}
		public PanelStyle PanelStyle
		{
			get { return this.Panel.PanelStyle; }
			set { SetProperty("PanelStyle", value); }
		}
		public ColorScheme ColorScheme
		{
			get { return this.Panel.ColorScheme; }
			set { SetProperty("ColorScheme", value); }
		}
		#endregion

		#region MethodsPublic

		public PanelDesignerActionList(System.ComponentModel.IComponent component)
	        : base(component)
	    {
	        // Automatically display smart tag panel when
	        // design-time component is dropped onto the
	        // Windows Forms Designer
	        base.AutoShow = true;
	    }

	    public override DesignerActionItemCollection GetSortedActionItems()
	    {
	        // Create list to store designer action items
	        DesignerActionItemCollection actionItems = new DesignerActionItemCollection();

	        actionItems.Add(
	          new DesignerActionMethodItem(
	            this,
	            "ToggleDockStyle",
	            GetDockStyleText(),
	            "Design",
	            "Dock or undock this control in it's parent container.",
	            true));

			actionItems.Add(
				new DesignerActionPropertyItem(
				"ShowTransparentBackground",
				"Show transparent backcolor",
				GetCategory(this.Panel, "ShowTransparentBackground")));

			actionItems.Add(
				new DesignerActionPropertyItem(
				"ShowXPanderPanelProfessionalStyle",
				"Show the XPanderPanels professional colorscheme",
				GetCategory(this.Panel, "ShowXPanderPanelProfessionalStyle")));

			actionItems.Add(
				new DesignerActionPropertyItem(
				"PanelStyle",
				"Select PanelStyle",
				GetCategory(this.Panel, "PanelStyle")));

			actionItems.Add(
			   new DesignerActionPropertyItem(
			   "ColorScheme",
			   "Select ColorScheme",
			   GetCategory(this.Panel, "ColorScheme")));

	        return actionItems;
	    }

	    // Dock/Undock designer action method implementation
	    //[CategoryAttribute("Design")]
	    //[DescriptionAttribute("Dock/Undock in parent container.")]
	    //[DisplayNameAttribute("Dock/Undock in parent container")]
	    public void ToggleDockStyle()
	    {

	        // Toggle ClockControl's Dock property
			if (this.Panel.Dock != DockStyle.Fill)
	        {
	            SetProperty("Dock", DockStyle.Fill);
	        }
	        else
	        {
	            SetProperty("Dock", DockStyle.None);
	        }
		}

		#endregion

		#region MethodsPrivate

		// Helper method that returns an appropriate
	    // display name for the Dock/Undock property,
	    // based on the ClockControl's current Dock 
	    // property value
	    private string GetDockStyleText()
	    {
			if (this.Panel.Dock == DockStyle.Fill)
	        {
	            return "Undock in parent container";
	        }
	        else
	        {
	            return "Dock in parent container";
	        }
	    }

	    private Panel Panel
	    {
			get { return (Panel)this.Component; }
	    }

	    // Helper method to safely set a componentís property
	    private void SetProperty(string propertyName, object value)
	    {
	        // Get property
	        System.ComponentModel.PropertyDescriptor property
				= System.ComponentModel.TypeDescriptor.GetProperties(this.Panel)[propertyName];
	        // Set property value
			property.SetValue(this.Panel, value);
		}

		// Helper method to return the Category string from a
		// CategoryAttribute assigned to a property exposed by 
		//the specified object
		private static string GetCategory(object source, string propertyName)
		{
			System.Reflection.PropertyInfo property = source.GetType().GetProperty(propertyName);
			CategoryAttribute attribute = (CategoryAttribute)property.GetCustomAttributes(typeof(CategoryAttribute), false)[0];
			if (attribute == null) return null;
			return attribute.Category;
		}

		#endregion
	}

	#endregion
}
