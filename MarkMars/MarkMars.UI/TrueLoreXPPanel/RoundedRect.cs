using System ;
using System.Drawing ;
using System.Drawing.Drawing2D ;


namespace MarkMars.UI.TrueLoreXPPanel
{
	[Flags]
	public enum CornerType {
		None = 0,
		TopLeft = 1,
		TopRight = 2,
		Top = TopLeft | TopRight,
		BottomLeft = 4,
		BottomRight = 8,
		Bottom = BottomLeft | BottomRight,
		Right = TopRight | BottomRight,
		Left = TopLeft | BottomLeft,
		All = Top | Bottom
	}

	/// <summary>
	/// Summary description for RoundedRect.
	/// </summary>
	public abstract class RoundedRect {
		public static GraphicsPath CreatePath(
										RectangleF rect, 
										int cornerRadius,
										int margin, 
										CornerType corners
										) 
		{
			GraphicsPath graphicsPath = new GraphicsPath() ;

			float xOffset = rect.X + margin ;
			float yOffset = rect.Y + margin ;
			float xExtent = rect.X + rect.Width - margin ;
			float yExtent = rect.Y + rect.Height - margin ;
			int diameter = cornerRadius << 1 ;
		
			// top arc																																																																																																
			if ((corners & CornerType.TopLeft) != 0) {
				graphicsPath.AddArc(new RectangleF(xOffset, yOffset, diameter, diameter), 180, 90) ;
			} else {
				graphicsPath.AddLine(new PointF(xOffset, yOffset + cornerRadius), new PointF(xOffset, yOffset)) ;
				graphicsPath.AddLine(new PointF(xOffset, yOffset), new PointF(xOffset + cornerRadius, yOffset)) ;
			}
			
			// top line
			graphicsPath.AddLine(new PointF(xOffset + cornerRadius, yOffset), new PointF(xExtent - cornerRadius, yOffset)) ;

			// top right arc
			if ((corners & CornerType.TopRight) != 0)
				graphicsPath.AddArc(new RectangleF(xExtent - diameter, yOffset, diameter,diameter), 270, 90) ;
			else {
				graphicsPath.AddLine(new PointF(xExtent - cornerRadius, yOffset), new PointF(xExtent, yOffset)) ;
				graphicsPath.AddLine(new PointF(xExtent, yOffset), new PointF(xExtent, yOffset + cornerRadius)) ;
			}

			// right line
			graphicsPath.AddLine(new PointF(xExtent, yOffset + cornerRadius), new PointF(xExtent, yExtent - cornerRadius)) ;

			// bottom right arc
			if ((corners & CornerType.BottomRight) != 0)
				graphicsPath.AddArc(new RectangleF(xExtent - diameter, yExtent - diameter, diameter,diameter), 0, 90) ;
			else {
				graphicsPath.AddLine(new PointF(xExtent, yExtent - cornerRadius),new PointF(xExtent, yExtent)) ;
				graphicsPath.AddLine(new PointF(xExtent, yExtent),new PointF(xExtent - cornerRadius, yExtent)) ;
			}

			// bottom line
			graphicsPath.AddLine(new PointF(xExtent - cornerRadius, yExtent), new PointF(xOffset + cornerRadius, yExtent)) ;

			// bottom left arc
			if ((corners & CornerType.BottomLeft) != 0)
				graphicsPath.AddArc(new RectangleF(xOffset, yExtent - diameter,diameter,diameter), 90, 90) ;
			else {
				 graphicsPath.AddLine(new PointF(xOffset + cornerRadius, yExtent), new PointF(xOffset, yExtent)) ;
				 graphicsPath.AddLine(new PointF(xOffset, yExtent), new PointF(xOffset, yExtent - cornerRadius)) ;
			}
			

			// left line
			graphicsPath.AddLine(new PointF(xOffset, yExtent - cornerRadius), new PointF(xOffset, yOffset + cornerRadius)) ;

			graphicsPath.CloseFigure() ;
			return graphicsPath ;
		}

		public static GraphicsPath CreatePath(
										RectangleF rect, 
										int cornerRadius,
										int margin
										) 
		{
			return CreatePath(rect,cornerRadius,margin,CornerType.All) ;
		}

		public static GraphicsPath CreatePath(RectangleF rect, int cornerRadius) {
			return CreatePath(rect,cornerRadius,1,CornerType.All) ;
		}

		public static GraphicsPath CreatePath(Rectangle rect, int cornerRadius) {
			return CreatePath(new RectangleF(rect.X,rect.Y,rect.Width,rect.Height),cornerRadius,1,CornerType.All) ;
		}
	}
}
