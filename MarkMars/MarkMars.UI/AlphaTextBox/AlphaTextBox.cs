using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using System.IO;
using System.Timers;

namespace MarkMars.UI
{
	#region AlphaTextBox Class

	/// <summary>
	/// Creates a TextBox that can have a Transparent Background.
	/// </summary>
	public class AlphaTextBox:TextBox
	{
		public AlphaTextBox()
		{
			InitializeComponent();
		}//default constructor

		#region Private Globals
		private delegate void SMDel(ref Message M);
		private Container components;
		private AlphaPanel APanel;
		private Utilities TBUtils;
		private Bitmap ClientRegionBitmap;
		private PointF CaretPosition;
		private System.Timers.Timer BlinkCaretTimer;
		private bool DrawCaret;
		private bool SelectingText;
		private Color InternalAlphaBackColor;
		private bool InternalBGSet;
		private int InternalAlphaAmount;
		#endregion

		#region Protected Globals
		//This is a delegate so the AlphaPanel can 
		//pass mouse messages to the AlphaTextBox
		protected internal Delegate STClientDel;

		#endregion

		#region Private Methods

		private void InitializeComponent()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			components=new Container();
			STClientDel=new SMDel(this.DefWndProc);
			TBUtils=new Utilities(STClientDel, this);

			InternalBGSet=false;
			DrawCaret=false;
			SelectingText=false;
			APanel=new AlphaPanel(this);

			BlinkCaretTimer=new System.Timers.Timer(500);
			BlinkCaretTimer.Elapsed+=new ElapsedEventHandler(BlinkCaretTimer_Elapsed);
			BlinkCaretTimer.AutoReset=true;

			this.BackColor=Color.Transparent;
			this.Controls.Add(APanel);
		}//InitializeComponent

		/// <summary>
		/// Captures the ClientRegion ONLY of this AlphaTextBox. (Text Area)
		/// </summary>
		/// <returns>Returns a Bitmap containing the ClientRegion.</returns>
		private Bitmap CaptureClientRegion()
		{
			InternalBGSet=true;
			Bitmap client=new Bitmap(this.Width, this.Height);
			this.BackColor=AlphaBackColor;
			Graphics g=Graphics.FromImage(client);
			IntPtr intp=g.GetHdc();

			//get the client region
			TBUtils.SendMessageToMaster(TBUtils.WM_PRINT, intp, (IntPtr)(TBUtils.PRF_CLIENT|TBUtils.PRF_ERASEBKGND), -1);
			g.ReleaseHdc(intp);
			g.Dispose();
		
			InternalBGSet=false;
			this.BackColor=Color.Transparent;
			return client;
		}//CaptureClientRegion

		/// <summary>
		/// Captures the NonClient area of the AlphaTextBox (Border and ScrollBars)
		/// </summary>
		/// <returns>Returns a Bitmap containing the NonClientRegion</returns>
		private Bitmap CaptureNonClientRegion()
		{
			Bitmap nonClient=new Bitmap(this.Width, this.Height);
			IntPtr intp;

			Graphics g=Graphics.FromImage(nonClient);
			intp=g.GetHdc();

			//get the non client region
			TBUtils.SendMessageToMaster(TBUtils.WM_PRINT, intp, (IntPtr)(TBUtils.PRF_NONCLIENT|TBUtils.PRF_ERASEBKGND), -1);
			g.ReleaseHdc(intp);
			g.Dispose();

			return nonClient;
		}//CaptureNonClientRegion

		/// <summary>
		/// Updates the ClientRegion of the AlphaTextBox and
		/// draws it to the screen.
		/// </summary>
		private void UpdateRegion()
		{
			if(ClientRegionBitmap!=null)
				ClientRegionBitmap.Dispose();

			ClientRegionBitmap=TBUtils.MapColors(AlphaBackColor, Color.FromArgb(AlphaAmount, AlphaBackColor), CaptureClientRegion(), true);
			APanel.BackgroundImage=(Bitmap)ClientRegionBitmap.Clone();

			//get the caret position
			SetCaret();

			GC.Collect();
		}//UpdateRegion

		/// <summary>
		/// Determines the CaretPosition in this AlphaTextBox.
		/// </summary>
		private void SetCaret()
		{
			//Digging around on Microsoft's Knowledge Base gives you a headache. :(

			//if the text is blank, set the cursor
			//to the default top left corner and return.
			if(this.Text=="")
			{
				CaretPosition=new PointF(2F, 2F);
				DrawCaretToBitmap();
				return;
			}//if

			int selectionIndex=this.SelectionStart+this.SelectionLength;
			bool lastIsNLine=false;
			float fntWidth=0;
			string textToMeasure="";
			int position=0;

			//the cursor should be at the end of the text, but no new line is there
			if(selectionIndex==this.TextLength && this.Text[this.Text.Length-1]!='\n')
				selectionIndex--;
			else
				if(selectionIndex==this.TextLength && this.Text[this.Text.Length-1]=='\n')
			{
				//the cursor is at the end of the text and a new line is there.
				//Decrement selection index to get the YCoordinate.
				lastIsNLine=true;
				selectionIndex--;
			}//else

			//The xCoordinate is stored in the low order word and the yCoordinate is 
			//stored in the high order word of the return value.  
			position=((IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_POSFROMCHAR, (IntPtr)selectionIndex, IntPtr.Zero, 1)).ToInt32();

			if(this.SelectionStart!=this.TextLength)
			{
				CaretPosition=new PointF((position&0xFF),(position>>16)&0xFF);
				DrawCaretToBitmap();
				return;
			}//if
			else
			{
				Graphics g=this.CreateGraphics();
				textToMeasure=this.Text[selectionIndex].ToString();
				fntWidth=g.MeasureString(textToMeasure, this.Font).Width;
				CaretPosition=new PointF((position&0xFF)+(fntWidth/2),(position>>16)&0xFF);
				g.Dispose();
			}//else

			if(lastIsNLine)
				CaretPosition=new PointF(2F, CaretPosition.Y+this.FontHeight);

			DrawCaretToBitmap();
		}//SetCursor

		/// <summary>
		/// Draws the Caret to the bitmap of the AlphaPanel object
		/// for this AlphaTextBox
		/// </summary>
		private void DrawCaretToBitmap()
		{
			Graphics g=Graphics.FromImage(APanel.BackgroundImage);

			//Draw the caret at CaretPosition
			if(DrawCaret)
				g.FillRectangle(new SolidBrush(this.ForeColor), CaretPosition.X, CaretPosition.Y, 
					this.Font.SizeInPoints/5, this.FontHeight);

			//otherwise copy ClientRegionBitmap back to APanel's background to erase the cursor
			else
				APanel.BackgroundImage=(Bitmap)ClientRegionBitmap.Clone();

			//Refresh the panel to add the update
			APanel.Refresh();
		}//DrawCaret

		private void BlinkCaretTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			//if selecting text, we dont want the cursor 
			//to display. (From SHIFT+<-OR->)
			if(!SelectingText && this.SelectionLength>0)
			{
				SelectingText=true;
				DrawCaret=false;
				UpdateRegion();
			}//if
			else
				if(!SelectingText)
			{
				DrawCaret=!DrawCaret;
				DrawCaretToBitmap();
			}//else
		}//BlinkcaretTimer Elapsed

		#endregion

		#region AlphaTextBox overrides
		/// <summary>
		/// This will always return Color.Transparent.
		/// Use AlphaBackColor to return the true BackColor.
		/// </summary>
		public override Color BackColor
		{
			//This override always sets the BackColor property to
			//transparent unless an internal edit is happening.

			get
			{
				return base.BackColor;
			}//get
			set
			{
				if(!InternalBGSet)
					base.BackColor = Color.Transparent;
				else
				{
					base.BackColor=value;
				}//else
			}//set
		}//override BackColor

		public override bool Multiline
		{
			get
			{
				return base.Multiline;
			}//get
			set
			{
				base.Multiline = value;
				UpdateRegion();
			}//set
		}//override Multiline

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components!=null)
					components.Dispose();
			}//if

			base.Dispose(disposing);
		}//dispose

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus (e);
			BlinkCaretTimer.Start();
			UpdateRegion();
		}//overrid OnGotFocus

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus (e);
			DrawCaret=false;
			UpdateRegion();
			BlinkCaretTimer.Stop();
		}//override OnLostFocus

		protected override void OnForeColorChanged(EventArgs e)
		{
			if(!InternalBGSet)
			{
				base.OnForeColorChanged (e);
				UpdateRegion();
			}//if
		}//override onForeColorChanged

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged (e);
			UpdateRegion();
		}//override OnFontChanged

		protected override void OnBorderStyleChanged(EventArgs e)
		{
			base.OnBorderStyleChanged (e);
			UpdateRegion();
		}//override OnBorderStyleChanged

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			APanel.Size=this.ClientSize;
			UpdateRegion();
		}//override OnSizeChanged

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged (e);
			APanel.Visible=this.Visible;
		}//On Visible Override

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);
			DrawCaret=true;

			if(!BlinkCaretTimer.Enabled)
				BlinkCaretTimer.Start();

			UpdateRegion();
		}//override OnTextChanged

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);

			if(this.SelectionLength==0 && SelectingText)
			{
				SelectingText=false;
				DrawCaret=true;
			}//if

			if(!BlinkCaretTimer.Enabled)
				BlinkCaretTimer.Start();

			UpdateRegion();
		}//override OnMouseUp

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);

			//We need this on KeyDown so if a key is held down, the
			//screen will scroll accordingly
			if(e.KeyCode==Keys.Left || e.KeyCode==Keys.Up || e.KeyCode==Keys.Down || e.KeyCode==Keys.Right ||
				e.KeyCode==Keys.PageDown || e.KeyCode==Keys.PageUp || e.KeyCode==Keys.Home || e.KeyCode==Keys.End)
			{
				DrawCaret=true;

				if(!BlinkCaretTimer.Enabled)
					BlinkCaretTimer.Start();

				UpdateRegion();
			}//if
		}//override OnKeyDown

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp (e);

			//only UpdateRegion under these conditions.
			//Keys Changing text will UpdateRegion in OnTextChanged
			if(e.KeyCode==Keys.Left || e.KeyCode==Keys.Up || e.KeyCode==Keys.Down || e.KeyCode==Keys.Right ||
				e.KeyCode==Keys.PageDown || e.KeyCode==Keys.PageUp || e.KeyCode==Keys.Home || e.KeyCode==Keys.End)
			{
				UpdateRegion();
			}//if
			else
				if(this.SelectionLength==0 && SelectingText)
			{
				SelectingText=false;
				DrawCaret=true;

				if(!BlinkCaretTimer.Enabled)
					BlinkCaretTimer.Start();

				UpdateRegion();
			}//if
		}//override OnKeyUp

		protected override void WndProc(ref Message m)
		{
			base.WndProc (ref m);

			if(m.Msg==TBUtils.WM_HSCROLL || m.Msg==TBUtils.WM_VSCROLL || m.Msg==TBUtils.WM_MOUSEWHEEL)
			{
				//stop the caret from being drawn when scrolling or it will end up
				//in strange places.
				DrawCaret=false;
				BlinkCaretTimer.Stop();
				UpdateRegion();
			}//if
			else
				if(m.Msg==TBUtils.WM_MOUSEMOVE && (int)m.WParam!=0)
			{
				//WParam holds key information, like mouse button click, etc.
				//if it is not 0, then update.
				DrawCaret=false;
				SelectingText=true;
				UpdateRegion();
			}//if
		}//override WndProc

		#endregion

		#region Public Methods and Variables

		//This is the amount of transparency applied to the background.
		//0=Totally transparent; 255=Totally Opaque
		[Category("Appearance"),Description("The Alpha Amount, or transparency amount, applied to the background."), Browsable(true),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

		public int AlphaAmount
		{
			get
			{
				return InternalAlphaAmount;
			}//get
			set
			{
				if(value>255 || value<0)
					throw(new Exception("AlphaAmount must be between 0 and 255!"));
				else
					InternalAlphaAmount=value;

				UpdateRegion();
			}//set
		}//AlphaAmount

		//setting the BackColor property will not do anything for the AlphaTextBox because it is always
		//set to be transparent.  You must set AlphaBackColor instead.
		[Category("Appearance"),Description("The visible background color for the AlphaTextBox."), Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

		public Color AlphaBackColor
		{
			get
			{
				return InternalAlphaBackColor;
			}//get
			set
			{
				InternalAlphaBackColor=value;
				UpdateRegion();
			}//set
		}//AlphaBackColor

		/// <summary>
		/// Returns a "screen shot" of the AlphaTextBox
		/// </summary>
		/// <returns>Returns a Bitmap containing the "screen shot"</returns>
		public Bitmap GetScreenShot()
		{
			Bitmap tmpB=CaptureNonClientRegion();
			Bitmap tmpB2=TBUtils.MapColors(AlphaBackColor, Color.FromArgb(AlphaAmount, AlphaBackColor), CaptureClientRegion(), true);
			Graphics g=Graphics.FromImage(tmpB);
			g.DrawImage(tmpB2, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
			g.Dispose();
			tmpB2.Dispose();

			return tmpB;
		}//GetScreenShot
		#endregion

		#region AlphaPanel Class

		private class AlphaPanel:Panel
		{
			protected internal AlphaPanel(AlphaTextBox Master)
			{
				MasterTb=Master;
				this.Size=Master.Size;
				this.Location=new Point(0, 0);

				ToMasterDel=MasterTb.STClientDel;
			
				//panels by default can have transparent backcolors
				//so you dont need to set that control style
				this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				this.SetStyle(ControlStyles.UserPaint, true);
				this.SetStyle(ControlStyles.DoubleBuffer,true);
				this.SetStyle(ControlStyles.Selectable, false);

				PUtils=new Utilities(ToMasterDel, MasterTb);
			}//AlphaPanel Constructor For AlphaTextBox

			private AlphaTextBox MasterTb;
			private Utilities PUtils;
			private delegate void SMDel(ref Message M); 
			protected internal Delegate ToMasterDel;

			protected override void WndProc(ref Message m)
			{
				base.WndProc (ref m);

				//All mouse messages have to be passed to the Master Edit Control
				//because the panel intercepts them.  
				if(m.Msg==PUtils.WM_MOUSEMOVE) 
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else			
					if(m.Msg==PUtils.WM_LBUTTONDOWN)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_LBUTTONUP)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_LBUTTONDBLCLK)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_MOUSELEAVE)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_RBUTTONDOWN)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_MOUSEACTIVATE)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
			}//Override WndProc
		}//class AlphaPanel

		#endregion
	}//AlphaTextBox Class

	#endregion

	#region AlphaRichTextBox Class
	/// <summary>
	/// A RichTextBox that supports transparent backgrounds.
	/// </summary>
	public class AlphaRichTextBox:RichTextBox
	{
		/// <summary>
		/// Creates a new instance of AlphaRichTextBox.
		/// </summary>
		public AlphaRichTextBox()
		{
			InitializeComponent();
		}//default Constructor

		
		#region Private Globals
		private delegate void SMDel(ref Message M);
		private Container components;
		private AlphaPanel APanel;
		private Utilities TBUtils;
		private Bitmap ClientRegionBitmap;
		private PointF CaretPosition;
		private System.Timers.Timer BlinkCaretTimer;
		private bool DrawCaret;
		private RichTextInformationCollection SelectedRTInfoInternal;
		private Color InternalAlphaBackColor;
		private Color CaretColorInternal;
		private int InternalAlphaAmount;
		private int TmpSelStart;
		private int TmpSelLength;
		private bool Updating;
		private bool DrawingCaret;
		private bool Scrolling;
		private bool Ctrl;
		private bool Dlt;
		private bool NewLine;
		#endregion

		#region Protected Globals
		//This is a delegate so the AlphaPanel can 
		//pass mouse messages to the AlphaTextBox
		protected internal Delegate STClientDel;

		#endregion

		#region Private Methods
		private void InitializeComponent()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			components=new Container();
			STClientDel=new SMDel(this.DefWndProc);
			TBUtils=new Utilities(STClientDel, this);

			DrawCaret=true;
			Updating=false;
			DrawingCaret=false;
			Scrolling=false;
			Ctrl=false;
			NewLine=false;
			APanel=new AlphaPanel(this);

			TmpSelStart=-1;

			BlinkCaretTimer=new System.Timers.Timer(500);
			BlinkCaretTimer.Elapsed+=new ElapsedEventHandler(BlinkCaretTimer_Elapsed);
			BlinkCaretTimer.AutoReset=true;
			BlinkCaretTimer.Enabled=false;

			SelectedRTInfoInternal=new RichTextInformationCollection();
			this.Controls.Add(APanel);
		}//InitializeComponent

		private void UpdateRegion()
		{
			if(Updating)
				return;

			//Disable redrawing of the control
			//If you dont do this, it will show scrolling.
			IntPtr eMask=(IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero, 1);
			TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero, -1);

			Updating=true;

			WriteText();

			if(TmpSelLength==0)
				WriteBitmap();

			//allow the control to be redrawn again
			TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero, -1);
			TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, eMask, -1);

			if(TmpSelLength>0)
				WriteBitmap();

			if(DrawCaret && TmpSelLength==0 && !Scrolling)
				SetCaret();
				
			Updating=false;
		}//UpdateRegion

		/// <summary>
		/// Writes the text to the bitmap using the selection colors, etc.
		/// </summary>
		private void WriteText()
		{
			int tmpS=this.SelectionStart, tmpSL=this.SelectionLength;

			//Dont blink if text is selected.
			if(tmpSL>0)
			{
				DrawCaret=false;
				BlinkCaretTimer.Stop();
			}//if
			else
			{
				if(this.Focused)
				{
					DrawCaret=true;
					BlinkCaretTimer.Start();
				}//if
			}//else

			if(SelectedRTInfoInternal.Count!=0 && !Dlt)
			{
				for(int i=0; i<TmpSelLength; i++)
				{
					int pos=i+TmpSelStart;
					this.Select(pos, 1);

					//reset the char to its initial value
					TBUtils.SetRTHighlight(SelectedRTInfoInternal[i].BackColor, SelectedRTInfoInternal[i].ForeColor);
				}//for
				
				//go back to the initial selection
				this.Select(tmpS, tmpSL);
			}//if

			TmpSelStart=tmpS;
			TmpSelLength=tmpSL;

			if(tmpSL>0 && !Dlt)
			{	
				SelectedRTInfoInternal.Clear();

				for(int i=0; i<tmpSL; i++)
				{
					this.Select(i+tmpS, 1);
					Color tmpFore=Color.Empty, tmpBack=Color.Empty;
					Font tmpFnt=null;
					TBUtils.GetRTHighlight(ref tmpBack, ref tmpFore, AlphaBackColor);
					tmpFnt=this.SelectionFont;
					SelectedRTInfoInternal.Add(new RichTextInformation(tmpFnt, tmpFore, tmpBack));
					TBUtils.SetRTHighlight(SystemColors.Highlight, SystemColors.HighlightText);
				}//for
			}//if
			else
				if(Dlt)
			{
				Color fore=SelectedRTInfoInternal[SelectedRTInfoInternal.Count-1].ForeColor;
				Color back=SelectedRTInfoInternal[SelectedRTInfoInternal.Count-1].BackColor;

				SelectedRTInfoInternal.Clear();

				this.SelectionColor=fore;
				this.SelectionBackColor=back;
				Dlt=false;
			}//else
			else
			{
				SelectedRTInfoInternal.Clear();
			}//else

			this.Select(TmpSelStart, TmpSelLength);
		}//WriteText

		private void WriteBitmap()
		{
			base.BackColor=AlphaBackColor;

			Bitmap tmpBmp=new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			Graphics g=Graphics.FromImage(tmpBmp);

			int firstPos=this.GetCharIndexFromPosition(new Point(2, 2));
			int lastPos=this.GetCharIndexFromPosition(new Point(this.ClientSize.Width, this.ClientSize.Height))+1;

			TBUtils.FormatRange(g, firstPos, lastPos);
			ClientRegionBitmap=TBUtils.MapRichTextColors(AlphaBackColor, Color.FromArgb(AlphaAmount, AlphaBackColor), tmpBmp, true);

			base.BackColor=Color.Transparent;
			APanel.BackgroundImage=(Bitmap)ClientRegionBitmap.Clone();
		}//WriteBitmap

		/// <summary>
		/// Determines the CaretPosition in this AlphaRichTextBox.
		/// </summary>
		private void SetCaret()
		{
			//if the text is blank, set the cursor
			//to the default top left corner and return.
			if(this.Text=="")
			{
				CaretPosition=new PointF(2F, 2F);
				DrawCaretToBitmap();
				return;
			}//if

			int selectionIndex=this.SelectionStart+this.SelectionLength;
			bool lastIsNLine=false;
			float fntWidth=0;
			string textToMeasure="";
			int position=0;

			//the cursor should be at the end of the text, but no new line is there
			if(selectionIndex==this.TextLength && this.Text[this.Text.Length-1]!='\n')
				selectionIndex--;
			else
				if(selectionIndex==this.TextLength && this.Text[this.Text.Length-1]=='\n')
			{
				//the cursor is at the end of the text and a new line is there.
				//Decrement selection index to get the YCoordinate.
				lastIsNLine=true;
				selectionIndex--;
			}//else

			//The xCoordinate is stored in the low order word and the yCoordinate is 
			//stored in the high order word of the return value.  
			position=((IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_POSFROMCHAR, (IntPtr)selectionIndex, IntPtr.Zero, 1)).ToInt32();

			if(TmpSelStart!=this.TextLength)
				CaretPosition=new PointF((position&0xFF),(position>>16)&0xFF);
			else
			{
				Graphics g=this.CreateGraphics();
				textToMeasure=this.Text[selectionIndex].ToString();
				fntWidth=g.MeasureString(textToMeasure, this.SelectionFont).Width;
				CaretPosition=new PointF((position&0xFF)+(fntWidth/2),(position>>16)&0xFF);
				g.Dispose();
			}//else

			if(lastIsNLine)
				CaretPosition=new PointF(2F, CaretPosition.Y+this.SelectionFont.Height);
			else
			{
				//get the first char in the current line, and the length of the current line
				int lineStart=(int)(IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_LINEINDEX, (IntPtr)(-1), IntPtr.Zero, 1);
				int lineEnd=lineStart+(int)(IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_LINELENGTH, (IntPtr)lineStart, IntPtr.Zero, 1);

				int height=0;

				//mark the selection properties before selecting new 
				//text or you wont ever be able to change anything.
				Color back=Color.Empty, fore=Color.Empty;
				TBUtils.GetRTHighlight(ref back, ref fore, AlphaBackColor);
				Font selFnt=this.SelectionFont;

				//get the tallest charachter in the line so you know 
				//how tall to draw the caret.
				while(lineStart<lineEnd)
				{
					this.Select(lineStart, 1);
					if(this.SelectionFont.Height>height)
						height=this.SelectionFont.Height;

					lineStart++;
				}//while
				

				//set the selectionProperties back
				this.Select(TmpSelStart, 0);
				base.SelectionFont=selFnt;
				TBUtils.SetRTHighlight(back, fore);

				//now get the baseline and subtract selFnt.Height to get the actual
				//size of the charachter.
				CaretPosition.Y=(CaretPosition.Y+height)-(height/5)-selFnt.Height;
			}//if

			NewLine=lastIsNLine;

			DrawCaretToBitmap();
		}//SetCursor

		/// <summary>
		/// Draws the Caret to the bitmap of the AlphaPanel object
		/// for this AlphaRichTextBox.
		/// </summary>
		private void DrawCaretToBitmap()
		{
			DrawingCaret=true;

			//Draw the caret at CaretPosition
			if(DrawCaret)
			{
				Graphics g=Graphics.FromImage(APanel.BackgroundImage);

				//this is a new line, simply draw the caret
				if(NewLine)
				{
					g.FillRectangle(CaretColorInternal.ToArgb()==Color.Empty.ToArgb() ? new SolidBrush(this.SelectionColor) :
						new SolidBrush(CaretColorInternal), 
						CaretPosition.X, CaretPosition.Y, 
						this.SelectionFont.SizeInPoints/5, this.SelectionFont.Height);

					g.Dispose();
				}//if
				else
				{

					g.FillRectangle(CaretColorInternal.ToArgb()==Color.Empty.ToArgb() ? new SolidBrush(this.SelectionColor) :
						new SolidBrush(CaretColorInternal), 
						CaretPosition.X, CaretPosition.Y, 
						this.SelectionFont.SizeInPoints/5, this.SelectionFont.Height);

					g.Dispose();
				}//else
			}//if
			else
				//otherwise copy ClientRegionBitmap back to APanel's background to erase the cursor
				APanel.BackgroundImage=(Bitmap)ClientRegionBitmap.Clone();

			APanel.Refresh();
			DrawingCaret=false;
		}//DrawCaret

		#endregion

		#region Overridden Properties/Methods
		private void BlinkCaretTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if(Updating || DrawingCaret || Scrolling)
				return;

			DrawCaret=!DrawCaret;
			DrawCaretToBitmap();
		}//BlinkcaretTimer Elapsed

		/// <summary>
		/// Always set to Transparent.  Use AlphaBackColor.
		/// </summary>
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}//get
			set
			{
				base.BackColor = Color.Transparent;
			}//set
		}//override BackColor

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged (e);

			if(!this.Focused)
			{
				DrawCaret=false;
				BlinkCaretTimer.Stop();
			}//if

			UpdateRegion();
		}//OnVisibleChanged

		/// <summary>
		/// Copy selected text.
		/// </summary>
		public new void Copy()
		{
			//We have to hide TextBoxBase.Copy here.  If not,
			//all that will be copied is the text with a backcolor
			//of Highlight and a forecolor of HighlightText.  

			//disable redrawing so the colors wont change
			IntPtr eMask=(IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero, 1);
			TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero, -1);

			//reset the selected text to its initial value
			RichTextInformationCollection tmp=SelectedRTInfoInternal;
			int tmpStart=TmpSelStart, tmpL=TmpSelLength;
			this.Select(this.TextLength, 0);
			WriteText();
			this.Select(tmpStart, tmpL);

			//put the formatted text/object on the clipboard
			Clipboard.SetDataObject(this.SelectedRtf, true);
			
			//set the text back to how it was
			SelectedRTInfoInternal=tmp;
			WriteText();

			//allow the control to be redrawn again
			TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero, -1);
			TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, eMask, -1);
		}//new Copy

		/// <summary>
		/// Past clipboard data.
		/// </summary>
		public new void Paste()
		{
			//See if the text is RTF or TEXT or other
			IDataObject toPaste=Clipboard.GetDataObject();
			if(toPaste.GetDataPresent(typeof(string)))
			{
				string tmp=(string)toPaste.GetData(typeof(string));
				if(tmp.StartsWith("{"))
					this.SelectedRtf=tmp;
				else
					this.SelectedText=tmp;
			}//if
			else
			{
				if(toPaste.GetDataPresent(typeof(Bitmap)))
				{
					//Pasting a bitmap with our AlphaBackColor will be transparent.
					//To fix the problem we will add 1 to the Red value of the bitmap
					//everywhere the color is AlphaBackColor.  There will be virtually 
					//no difference.
					Bitmap tmp=(Bitmap)toPaste.GetData(typeof(Bitmap));
					Clipboard.SetDataObject(
						TBUtils.MapColors(AlphaBackColor, Color.FromArgb(AlphaBackColor.R!=255 ? AlphaBackColor.R+1 : AlphaBackColor.R-1,
						AlphaBackColor.G, AlphaBackColor.B),
						tmp, true), true);
				}//if
				
				base.Paste();
			}//else

			UpdateRegion();
		}//new Paste

		public new void Paste(DataFormats.Format ClipFormat)
		{
			Paste();
		}//new paste

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			APanel.Size=this.ClientSize;
			if(ClientRegionBitmap!=null)
				ClientRegionBitmap.Dispose();

			ClientRegionBitmap=new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			UpdateRegion();
		}//OnSizeChanged

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp (e);
			Ctrl=false;
			UpdateRegion();
		}//OnkeyUp

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
			Scrolling=false;
			UpdateRegion();
		}//OnMouseUp

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus (e);
			BlinkCaretTimer.Stop();
			DrawCaret=false;
			DrawCaretToBitmap();
		}//OnLostFocus

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus (e);
			DrawCaret=true;
			BlinkCaretTimer.Start();
		}//OnGotFocus

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==TBUtils.WM_KEYDOWN)
			{
				//To keep from importing any DLL here,
				//I will capture the CTRL and C/V seperately.
				//On KeyUp, Ctrl will be set to false so 
				//that you cant press CTRL, then release and
				//type c and copy.  You could just as easily
				//import Win32.dll and use GetKeyState(int nVirtualKey)
				//to get the CTRL status.  WM_KEYDOWN does not tell 
				//you about CTRL, unless it is the Extended right CTRL,
				//but we want both.
				int key=m.WParam.ToInt32();

				//67 is the KeyCode value of C
				//86 is the KeyCode value of V
				//17 is the KeyCode value for CTRL
				//46 is the KeyCode value for DLT

				if(key==46 && SelectedRTInfoInternal.Count>0)//if delete and text is selected
				{
					Dlt=true;
					base.WndProc(ref m);
					return;
				}//if

				if(key==17)//Set CTRL flag.
				{
					Ctrl=true;
					base.WndProc (ref m);
					return;
				}//if
				
				if(key==67 && Ctrl)//C is pressed right after CTRL flag set.
				{
					Copy();
					Ctrl=false;
					return;
				}//if
				else
					if(key==86 && Ctrl)//P is pressed right after CTRL flag set.
				{
					Paste();
					Ctrl=false;
					return;
				}//else
				else//any other key and we unset the CTRL flag so if c or p is typed it doesnt copy.
					Ctrl=false;

				base.WndProc (ref m);
				Scrolling=false;
				UpdateRegion();
			}//if
			else
				if(m.Msg==TBUtils.WM_MOUSEMOVE)
			{
				int lParam=m.LParam.ToInt32();
				int wParam=m.WParam.ToInt32();

				//x coordinate is in low order byte lParam
				//y coordintat is in high order byte lParam

				if(wParam==TBUtils.MK_LBUTTON)
				{
					if(Updating)
					{
						base.WndProc(ref m);
						return;
					}//if

					int x=(lParam&0xFF);
					int y=(lParam>>16)&0xFF;

					int charIndex=this.GetCharIndexFromPosition(new Point(x, y))+1;

					//Right now backwards selecting is disabled.
					if(charIndex>this.SelectionStart)
						this.Select(this.SelectionStart, charIndex-this.SelectionStart);
					else
					{
						int t=charIndex+((TmpSelStart-charIndex)+TmpSelLength);

						if((this.SelectionStart+this.SelectionLength)==this.TextLength && t>0)
							this.Select(charIndex, t);
						else
							if((t-charIndex)>0)
							this.Select(charIndex, t-charIndex);
					}//else

					UpdateRegion();
					return;
				}//if

				base.WndProc(ref m);
			}//else
			else
				if(m.Msg==TBUtils.WM_VSCROLL)
			{
				base.WndProc(ref m);
				int wParam=m.WParam.ToInt32();
				Scrolling=true;
				DrawCaret=false;

				//wParam has the scroll information
				if((wParam&TBUtils.SB_LINEUP)==0 || (wParam&TBUtils.SB_LINEDOWN)==0)
					WriteBitmap();
			}//if
			else
				if(m.Msg==TBUtils.WM_MOUSEWHEEL)
			{
				base.WndProc(ref m);
				Scrolling=true;
				DrawCaret=false;

				WriteBitmap();
			}//else
			else
				base.WndProc(ref m);
		}//wndproc
		#endregion

		#region New Properties

		[Category("Appearance"),Description("The Alpha Amount, or transparency amount, applied to the background."), Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

		/// <summary>
		// This is the amount of transparency applied to the background.
		// 0=Totally transparent; 255=Totally Opaque
		/// </summary>
		public int AlphaAmount
		{
			get
			{
				return InternalAlphaAmount;
			}//get
			set
			{
				if(value>255 || value<0)
					throw(new Exception("AlphaAmount must be between 0 and 255!"));
				else
					InternalAlphaAmount=value;

				UpdateRegion();
				APanel.Refresh();
			}//set
		}//AlphaAmount

		[Category("Appearance"),Description("The visible background color for the AlphaTextBox."), Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		/// <summary>
		/// Setting the BackColor property will not do anything for the AlphaRichTextBox because it is always
		//  set to be transparent.  You must set AlphaBackColor instead.
		/// </summary>
		public Color AlphaBackColor
		{
			get
			{
				return InternalAlphaBackColor;
			}//get
			set
			{	

				for(int i=0; i<SelectedRTInfoInternal.Count; i++)
				{
					if(SelectedRTInfoInternal[i].BackColorInternal.ToArgb()==InternalAlphaBackColor.ToArgb())
						SelectedRTInfoInternal[i].BackColorInternal=value;
				}//for

				for(int i=0; i<this.TextLength; i++)
				{
					this.Select(i, 1);
					if(this.SelectionBackColor.ToArgb()==InternalAlphaBackColor.ToArgb())
                        this.SelectionBackColor=value;
				}//for

				this.Select(TmpSelStart, TmpSelLength);
				InternalAlphaBackColor=value;

				UpdateRegion();
				APanel.Refresh();
			}//set
		}//AlphaBackColor

		/// <summary>
		/// Gets the RichTextInformationCollection containing
		/// the background and foreground colors and Fonts of
		/// each indavidual charachter in the selection.
		/// </summary>
		[Category("Appearance"),Description(""), Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RichTextInformationCollection SelectedRTInfo
		{
			get
			{
				return SelectedRTInfoInternal;
			}//get
		}//SelectionRTInfo

		/// <summary>
		/// Gets or sets the background color of all selected text.  
		/// If no text is selected it sets the background color at the
		/// insertion point.
		/// </summary>
		[Category("Appearance"),Description(""), Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color SelectionBackColor 
		{
			get
			{
				Color fore=Color.Empty, back=Color.Empty;
				TBUtils.GetRTHighlight(ref back, ref fore, Color.Empty);
				return back;
			}//get
			set
			{
				if(!this.Focused)
					this.Focus();

				TBUtils.SetRTHighlight(value, this.SelectionColor);

				for(int i=0; i<SelectedRTInfoInternal.Count; i++)
					SelectedRTInfoInternal[i].BackColorInternal=value;
			}//set
		}//SelectionBackColor

		/// <summary>
		/// Gets or sets the text color applied to the current selection.
		/// </summary>
		public new Color SelectionColor
		{
			get
			{
				return base.SelectionColor;
			}//get
			set
			{
				if(!this.Focused)
					this.Focus();

				base.SelectionColor=value;

				for(int i=0; i<SelectedRTInfoInternal.Count; i++)
					SelectedRTInfoInternal[i].ForeColorInternal=value;

				TBUtils.SetRTHighlight(this.SelectionBackColor, this.SelectionColor);
			}//set
		}//SelectionColor

		/// <summary>
		/// Gets or sets the font applied to the current selection.
		/// </summary>
		public new Font SelectionFont
		{
			get
			{
				return base.SelectionFont;
			}//get
			set
			{
				if(!this.Focused)
					this.Focus();

				for(int i=0; i<SelectedRTInfoInternal.Count; i++)
					SelectedRTInfoInternal[i].FontInternal=value;

				base.SelectionFont=value;

				UpdateRegion();
			}//set
		}//SelectionFont

		/// <summary>
		/// Gets or sets a value determining what color the caret will be drawn in.
		/// To draw the caret in whatever the fore color is, set to Color.Empty.
		/// Returns Color.Empty if the caret color is set to the fore color.
		/// </summary>
		public Color CaretColor
		{
			get
			{
				return CaretColorInternal;
			}//get
			set
			{
				CaretColorInternal=value;

				if(!this.Focused)
					this.Focus();
			}//set
		}//CaretColor
		#endregion

		#region AlphaPanel Class

		private class AlphaPanel:Panel
		{
			protected internal AlphaPanel(AlphaRichTextBox Master)
			{
				MasterTb=Master;
				this.Size=Master.Size;
				this.Location=new Point(0, 0);

				ToMasterDel=MasterTb.STClientDel;
			
				//panels by default can have transparent backcolors
				//so you dont need to set that control style
				this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				this.SetStyle(ControlStyles.UserPaint, true);
				this.SetStyle(ControlStyles.DoubleBuffer,true);
				this.SetStyle(ControlStyles.Selectable, false);

				PUtils=new Utilities(ToMasterDel, MasterTb);
			}//AlphaPanel Constructor For AlphaTextBox

			private AlphaRichTextBox MasterTb;
			private Utilities PUtils;
			private delegate void SMDel(ref Message M); 
			protected internal Delegate ToMasterDel;

			protected override void WndProc(ref Message m)
			{
				base.WndProc (ref m);

				//All mouse messages have to be passed to the Master Edit Control
				//because the panel intercepts them.  
				if(m.Msg==PUtils.WM_MOUSEMOVE) 
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else			
					if(m.Msg==PUtils.WM_LBUTTONDOWN)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_LBUTTONUP)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_LBUTTONDBLCLK)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_MOUSELEAVE)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_RBUTTONDOWN)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				else
					if(m.Msg==PUtils.WM_MOUSEACTIVATE)
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
			}//Override WndProc
		}//class AlphaPanel

		#endregion
	}//AlphaRichTextBox Class
	#endregion

	#region RichTextInformation Classes

	#region RichTextInformation class
	/// <summary>
	/// This class represents 1 charachter in a RichText selection.
	/// </summary>
	public class RichTextInformation
	{
		public RichTextInformation(Font fnt, Color fore, Color back)
		{
			FontInternal=fnt;
			ForeColorInternal=fore;
			BackColorInternal=back;
		}//Constructor

		public RichTextInformation()
		{}//Contstructor

		protected internal Font FontInternal;
		protected internal Color ForeColorInternal, BackColorInternal;

		/// <summary>
		/// Gets or sets the Font of the charachter.
		/// </summary>
		public Font Font
		{
			get
			{
				return FontInternal;
			}//get
		}//Font

		/// <summary>
		/// Gets or sets the foreground color of the charachter.
		/// </summary>
		public Color ForeColor
		{
			get
			{
				return ForeColorInternal;
			}//get
		}//ForeColor

		/// <summary>
		/// Gets or sets the backcolor of the charachter.
		/// </summary>
		public Color BackColor
		{
			get
			{
				return BackColorInternal;
			}//get
		}//BackColor
	}//RichTextInformation
	#endregion

	#region RichTextInformationCollection Class
	/// <summary>
	/// A collection of selection text inforamtion.
	/// </summary>
	public class RichTextInformationCollection:CollectionBase
	{
		/// <summary>
		/// Gets or sets the RichTextInformation at the 
		/// specified index.
		/// </summary>
		public RichTextInformation this[int index]
		{
			get
			{
				return (RichTextInformation)this.InnerList[index];
			}//get
			set
			{
				this.InnerList[index]=value;
			}//set
		}//[]

		/// <summary>
		/// Adds a new RichTextInformation object to the
		/// collection.
		/// </summary>
		/// <param name="value">The RichTextInformation object to add.</param>
		public void Add(RichTextInformation value)
		{
			this.InnerList.Add(value);
		}//Add

		/// <summary>
		/// Gets the number of charachters in the current selection.
		/// </summary>
		public new int Count
		{
			get
			{
				return this.InnerList.Count;
			}//get
		}//Count

		/// <summary>
		/// Clears the contents of the collection.
		/// </summary>
		public new void Clear()
		{
			this.InnerList.Clear();
		}//Clear
	}//RichTextInformationCollection
	#endregion
	#endregion

	#region Utilities Class
	public class Utilities
	{
		/// <summary>
		/// Constructor for Utilities
		/// </summary>
		/// <param name="DMaster">The delegate to the Master Control's DefWndPrc function.</param>
		/// <param name="Master">The Master Control.</param>
		protected internal Utilities(Delegate DMaster, Control Master)
		{
			MasterDelegate=DMaster;
			MasterControl=Master;
		}//Constructor

		#region Win32 Structs and methods
		[ StructLayout( LayoutKind.Sequential)]
		private struct STRUCT_RECT 
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}//STRUCT_RECT

		[ StructLayout( LayoutKind.Sequential)]
		private struct STRUCT_CHARRANGE
		{
			public int cpMin;
			public int cpMax;
		}//STRUCT_CHARRANGE

		[ StructLayout( LayoutKind.Sequential)]
		private struct STRUCT_FORMATRANGE
		{
			public IntPtr hdc; 
			public IntPtr hdcTarget; 
			public STRUCT_RECT rc; 
			public STRUCT_RECT rcPage; 
			public STRUCT_CHARRANGE chrg; 
		}//STRUCT FORMATRANGE

		[StructLayout(LayoutKind.Sequential)]
		private struct CHARFORMAT2 
		{
			public int cbSize;
			public int dwMask;
			public int dwEffects;
			public int yHeight;
			public int yOffset;
			public int crTextColor;
			public byte bCharSet;
			public byte bPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)] 
			public string szFaceName;
			public UInt16 wWeight;
			public UInt16 sSpacing;
			public int crBackColor;
			public int lcid;
			public int dwReserved;
			public UInt16 sStyle;
			public UInt16 wKerning;
			public byte bUnderlineType;
			public byte bAnimation;
			public byte bRevAuthor;
			public byte bReserved1;
		}//struct charformat2 

		private int ToTwips(float amt)
		{
			return (int)(amt*14.4F);
		}//ToTwips
		#endregion

		#region Private Globals

		private Control MasterControl;
		private Delegate MasterDelegate;
		private ColorMap[] CMap;
		private ImageAttributes IAttribs;

		#endregion

		#region Windows Message Constants (Thank You Google)

		protected internal readonly int EM_GETLINECOUNT=0xBA;
		protected internal readonly int EM_LINEINDEX=0xBB;
		protected internal readonly int EM_LINELENGTH=0xC1;
		protected internal readonly int EM_LINEFROMCHAR=0xC9;
		protected internal readonly int EM_GETSEL=0xB0;
		protected internal readonly int EM_GETFIRSTVISIBLELINE=0xCE;
		protected internal readonly int EM_SETEVENTMASK=0x431;
		protected internal readonly int EM_POSFROMCHAR=0xD6;
		protected internal readonly int EN_UPDATE=0x0400;
		protected internal readonly int EM_MOUSESELECT=0xFF;
		protected internal readonly int WM_PRINT=0x317;
		protected internal readonly int PRF_ERASEBKGND=0x8;
		protected internal readonly int PRF_CLIENT=0x4;
		protected internal readonly int PRF_NONCLIENT=0x2;
		protected internal readonly int WM_MOUSEMOVE=0x200;
		protected internal readonly int WM_LBUTTONDOWN=0x201;
		protected internal readonly int WM_LBUTTONUP=0x202;
		protected internal readonly int WM_RBUTTONDOWN=0x204;
		protected internal readonly int WM_LBUTTONDBLCLK=0x203;
		protected internal readonly int WM_MOUSELEAVE=0x2A3;
		protected internal readonly int WM_MOUSEACTIVATE=0x21;
		protected internal readonly int WM_HSCROLL=0x114;
		protected internal readonly int	WM_VSCROLL=0x115;
		protected internal readonly int WM_MOUSEWHEEL=0x20A;
		protected internal readonly int WM_SETREDRAW=0xB;
		protected internal readonly int WM_KEYDOWN=0x100;
		protected internal readonly int EM_FORMATRANGE=0x439;
		protected internal readonly int EM_SETCHARFORMAT=0x400+68;
		protected internal readonly int EM_GETCHARFORMAT=0x400+58;
		protected internal readonly int CFM_BACKCOLOR=0x4000000;
		protected internal readonly int CFM_COLOR=0x40000000;
		protected internal readonly int CFE_AUTOBACKCOLOR=0x4000000;
		protected internal readonly int SCF_SELECTION=0x1;
		protected internal readonly int MK_LBUTTON=0x1;
		protected internal readonly int SB_LINEUP=0x0;
		protected internal readonly int SB_LINEDOWN=0x1;
		#endregion

		#region Protected Internal Functions

		/// <summary>
		/// Draws the contents of the parent RichTextBox into the specified
		/// device context.
		/// </summary>
		/// <param name="g">The graphics object that will draw to the device context.</param>
		/// <param name="startChar">The first charachter index to capture.</param>
		/// <param name="endChar">The last charachter index to capture.</param>
		protected internal void FormatRange(Graphics g, int startChar, int endChar)
		{
			STRUCT_CHARRANGE charRange;
			charRange.cpMin=startChar;
			charRange.cpMax=endChar;

			STRUCT_RECT rc;
			rc.top=0;
			rc.bottom=ToTwips(MasterControl.ClientSize.Height+40);
			rc.left=0;

			if(MasterControl.Size.Width-MasterControl.ClientSize.Width==20)//VScrollbar present
				rc.right=ToTwips(MasterControl.ClientSize.Width+(MasterControl.ClientSize.Width/80F)+4);
			else
				//VScrollbar not present
				rc.right=ToTwips(MasterControl.ClientSize.Width+(MasterControl.ClientSize.Width/100F)+5);
	
			STRUCT_RECT rcPage;
			rcPage.top=0;
			rcPage.bottom=ToTwips(MasterControl.Size.Height);
			rcPage.left=0;
			rcPage.right=ToTwips(MasterControl.Size.Width);
			IntPtr hdc = g.GetHdc();

			//This is what specifies all the information
			//for drawing to the bitmap
			STRUCT_FORMATRANGE forRange;
			forRange.chrg=charRange;
			forRange.hdc=hdc;
			forRange.hdcTarget=hdc;
			forRange.rc=rc;
			forRange.rcPage=rcPage;

			//We have to send and IntPtr as the lParam.  You cant simply 
			//make a pointer to forRange, so allocate memory and Marshal
			//it to an IntPtr
			IntPtr lParam=Marshal.AllocCoTaskMem(Marshal.SizeOf(forRange)); 
			Marshal.StructureToPtr(forRange, lParam, false);

			SendMessageToMaster(EM_FORMATRANGE, (IntPtr)1, lParam, 1);
			SendMessageToMaster(EM_FORMATRANGE, IntPtr.Zero, IntPtr.Zero, -1);

			//release resources
			Marshal.FreeCoTaskMem(lParam);
			g.ReleaseHdc(hdc);
		}//FormatRange

		/// <summary>
		/// Sets the highlight of the selected RichText.
		/// Should only be used for 1 charachter.
		/// </summary>
		/// <param name="back">The bacground color to use.</param>
		/// <param name="fore">The text color to use.</param>
		protected internal void SetRTHighlight(Color back, Color fore)
		{
			CHARFORMAT2 cf2=new CHARFORMAT2();
            cf2.dwMask=(CFM_BACKCOLOR|CFM_COLOR);
			cf2.cbSize=Marshal.SizeOf(cf2);
			cf2.crBackColor=ColorTranslator.ToWin32(back);
			cf2.crTextColor=ColorTranslator.ToWin32(fore);

			IntPtr lParam=Marshal.AllocCoTaskMem(cf2.cbSize); 
			Marshal.StructureToPtr(cf2, lParam, false);

			SendMessageToMaster(EM_SETCHARFORMAT, (IntPtr)SCF_SELECTION, lParam, 1);

			Marshal.FreeCoTaskMem(lParam);
		}//SetRTHighlight

		/// <summary>
		/// Gets the background and foreground color of selected RichText.
		/// Should only be used for 1 charachter.
		/// </summary>
		/// <param name="back">Will recieve the background color.</param>
		/// <param name="fore">Will recieve the Text Color.</param>
		/// <param name="AlphaBackColor">The color to return if the back color is transparent.</param>
		protected internal void GetRTHighlight(ref Color back, ref Color fore, Color AlphaBackColor)
		{
			CHARFORMAT2 cf2=new CHARFORMAT2();
			cf2.cbSize=Marshal.SizeOf(cf2);

			IntPtr lParam=Marshal.AllocCoTaskMem(cf2.cbSize);
			Marshal.StructureToPtr(cf2, lParam, false);

			//return lParam so you can get the modified version
			lParam=(IntPtr)SendMessageToMaster(EM_GETCHARFORMAT, (IntPtr)SCF_SELECTION, lParam, 3);

			//assign the modified memory contents to cf2 so the
			//properties will be available
			cf2=(CHARFORMAT2)Marshal.PtrToStructure(lParam, typeof(CHARFORMAT2));

			//if crBackColor is 0, it is probably picking up transparency, so 
			//set crBackColor to AlphaBackColor so it can be blended.
			back=cf2.crBackColor==0 ? AlphaBackColor : ColorTranslator.FromWin32(cf2.crBackColor);
			fore=ColorTranslator.FromWin32(cf2.crTextColor);

			Marshal.FreeCoTaskMem(lParam);
		}//GetRTHighlight

		/// <summary>
		/// This sends a message to the Master AlphaControl (The control
		/// this panel is owned by) via the MasterDelegate function.
		/// </summary>
		/// <param name="Msg">An integer specifying the Windows Message to send.</param>
		/// <param name="WParams">An IntPtr specifying the WParameters.</param>
		/// <param name="LParams">An IntPtr specifying the LParameters</param>
		/// <param name="ReturnInstance">Returns: 0: The Message (Message)
		/// 1: Message Return Value (IntPtr) 2: WParams (IntPtr) 3: LParams (IntPtr)</param>
		/// <returns>An object specified by ReturnInstance.  If ReturnInstance is not acceptable, -1 is returned.</returns>
		protected internal object SendMessageToMaster(int Msg, IntPtr WParams, IntPtr LParams, int ReturnInstance)
		{
			Message m=Message.Create(MasterControl.Handle, Msg, WParams, LParams);
			object[] vals={m};
			MasterDelegate.DynamicInvoke(vals);

			switch(ReturnInstance)
			{
				case 0:
					return vals[0];
				
				case 1:
					return ((Message)vals[0]).Result;

				case 2:
					return ((Message)vals[0]).WParam;

				case 3:
					return ((Message)vals[0]).LParam;

				default:
					return -1;
			}//switch
		}//SendMessageToMaster

		/// <summary>
		/// Draws the Color specified by New at every instance of the Color specified by Old
		/// according to the Bitmap specified by Bmp.
		/// </summary>
		/// <param name="Old">The Color to be replace.</param>
		/// <param name="New">The Color with which to replace Old.</param>
		/// <param name="Bmp">The Bitmap that needs to be mapped.</param>
		/// <param name="Dspse">true to dispose of Bmp, otherwise false.</param>
		/// <returns>Returns a copy of Bmp with the Color New mapped to Color Old.</returns>
		protected internal Bitmap MapColors(Color Old, Color New, Bitmap Bmp, bool Dspse)
		{
			Bitmap tmpBmp=new Bitmap(Bmp.Width, Bmp.Height);
			Graphics g=Graphics.FromImage(tmpBmp);
			CMap=new ColorMap[1];
			CMap[0]=new ColorMap();
			CMap[0].OldColor=Old;
			CMap[0].NewColor=New;
			IAttribs=new ImageAttributes();
			IAttribs.SetRemapTable(CMap, ColorAdjustType.Bitmap);

			g.DrawImage(Bmp, new Rectangle(new Point(0, 0), new Size(Bmp.Width, Bmp.Height)), 0, 0, 
				Bmp.Width, Bmp.Height, GraphicsUnit.Pixel, IAttribs);
			g.Dispose();

			if(Dspse)
				Bmp.Dispose();

			return tmpBmp;
		}//MapColors

		/// <summary>
		/// The RichTextBox version of MapColors.
		/// </summary>
		/// <param name="Old">The old color to change.</param>
		/// <param name="New">The color to make the Old color.</param>
		/// <param name="Bmp">The bitmap that needs its colors mapped.</param>
		/// <param name="Dspse">Weather to dispose Bmp or not.</param>
		/// <returns></returns>
		protected internal Bitmap MapRichTextColors(Color Old, Color New, Bitmap Bmp, bool Dspse)
		{
			Bitmap tmpBmp=new Bitmap(MasterControl.ClientSize.Width, MasterControl.ClientSize.Height);
			Graphics g=Graphics.FromImage(tmpBmp);
			g.Clear(New);

			CMap=new ColorMap[1];
			CMap[0]=new ColorMap();
			CMap[0].OldColor=Old;
			CMap[0].NewColor=Color.Transparent;
			IAttribs=new ImageAttributes();
			IAttribs.SetRemapTable(CMap, ColorAdjustType.Bitmap);

			g.DrawImage(Bmp, new Rectangle(new Point(0, 0), new Size(Bmp.Width, Bmp.Height)), 0, 0, 
				Bmp.Width, Bmp.Height, GraphicsUnit.Pixel, IAttribs);
			g.Dispose();

			if(Dspse)
				Bmp.Dispose();

			return tmpBmp;
		}//MapColors

		#endregion
	}//Class Utilities

	#endregion
}//AlphaUtils
