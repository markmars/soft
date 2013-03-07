using System;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.UI.TrueLorePanel
{
    /// <summary>
	/// Contains information for the drawing of panels or xpanderpanels in a xpanderpanellist. 
    /// </summary>
	/// <remarks>
	/// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
	/// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
	/// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
	/// PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
	/// REMAINS UNCHANGED.
	/// </remarks>
	/// <copyright>Copyright ?2006-2007 Uwe Eichkorn</copyright>
	public enum ColorScheme
    {
        /// <summary>
        /// draws the panels caption with <see cref="ProfessionalColors">ProfessionalColors</see>
        /// </summary>
		Professional,
        /// <summary>
        /// draws the panels caption with custom colors
        /// </summary>
		Custom
    }
}
