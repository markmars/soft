namespace MarkMars.UI.TabStrip
{
    /// <summary>
    /// Hit test result of <see cref="FATabStrip"/>
    /// </summary>
    public enum HitTestResult
    {
        //CloseButton,
        MenuGlyph,
        TabItem,
        None
    }
    
    /// <summary>
    /// Theme Type
    /// </summary>
    public enum ThemeTypes
    {
        WindowsXP,
        Office2000,
        Office2003,
        Office2007,
        Windows7
    }

    /// <summary>
    /// Indicates a change into TabStrip collection
    /// </summary>
    public enum FATabStripItemChangeTypes
    {
        Added,
        Removed,
        Changed,
        SelectionChanged
    }
}
