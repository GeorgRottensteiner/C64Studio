using System;

namespace Be.Windows.Forms
{
	public interface ISelectedByteProvider
	{
		bool IsByteSelected(long index);

    void SetByteSelectionState( long index, bool Selected );

    void InsertBytes( long index, long length );
    void DeleteBytes( long index, long length );
	}
}
