using System;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
using LibGit2Sharp;
#endif

namespace SourceControl
{
  // direct copy of LibGitSharp enum
  [Flags]
  public enum FileState
  {
    //
    // Summary:
    //     The file doesn't exist.
    Nonexistent = int.MinValue,
    //
    // Summary:
    //     The file hasn't been modified.
    Unaltered = 0x0,
    //
    // Summary:
    //     New file has been added to the Index. It's unknown from the Head.
    NewInIndex = 0x1,
    //
    // Summary:
    //     New version of a file has been added to the Index. A previous version exists
    //     in the Head.
    ModifiedInIndex = 0x2,
    //
    // Summary:
    //     The deletion of a file has been promoted from the working directory to the Index.
    //     A previous version exists in the Head.
    DeletedFromIndex = 0x4,
    //
    // Summary:
    //     The renaming of a file has been promoted from the working directory to the Index.
    //     A previous version exists in the Head.
    RenamedInIndex = 0x8,
    //
    // Summary:
    //     A change in type for a file has been promoted from the working directory to the
    //     Index. A previous version exists in the Head.
    TypeChangeInIndex = 0x10,
    //
    // Summary:
    //     New file in the working directory, unknown from the Index and the Head.
    NewInWorkdir = 0x80,
    //
    // Summary:
    //     The file has been updated in the working directory. A previous version exists
    //     in the Index.
    ModifiedInWorkdir = 0x100,
    //
    // Summary:
    //     The file has been deleted from the working directory. A previous version exists
    //     in the Index.
    DeletedFromWorkdir = 0x200,
    //
    // Summary:
    //     The file type has been changed in the working directory. A previous version exists
    //     in the Index.
    TypeChangeInWorkdir = 0x400,
    //
    // Summary:
    //     The file has been renamed in the working directory. The previous version at the
    //     previous name exists in the Index.
    RenamedInWorkdir = 0x800,
    //
    // Summary:
    //     The file is unreadable in the working directory.
    Unreadable = 0x1000,
    //
    // Summary:
    //     The file is LibGit2Sharp.FileStatus.NewInWorkdir but its name and/or path matches
    //     an exclude pattern in a gitignore file.
    Ignored = 0x4000,
    //
    // Summary:
    //     The file is LibGit2Sharp.FileStatus.Conflicted due to a merge.
    Conflicted = 0x8000
  }



}
