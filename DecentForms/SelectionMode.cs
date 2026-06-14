namespace DecentForms
{
  public enum SelectionMode
  {
    /// <summary>
    ///  Indicates that no items can be selected.
    /// </summary>
    NONE = 0,

    /// <summary>
    ///  Indicates that only one item at a time can be selected.
    /// </summary>
    ONE = 1,

    /// <summary>
    ///  Indicates that more than one item at a time can be selected, and
    ///  keyboard combinations, such as SHIFT and CTRL can be used to help
    ///  in selection.
    /// </summary>
    MULTI = 2
  }
}
