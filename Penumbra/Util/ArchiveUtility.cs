using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace Penumbra.Util;

/// <summary>Compatibility helpers for archive formats with different extraction models.</summary>
public static class ArchiveUtility
{
    /// <summary>
    /// Iterate solid and 7z archives through their shared reader, while opening entries from
    /// non-solid archives individually. SharpCompress 0.48 rejects ExtractAllEntries for the
    /// latter even though older versions accepted it.
    /// </summary>
    public static void ForEachEntry(IArchive archive, Action<ReaderShim> action)
    {
        if (archive.IsSolid || archive.Type is ArchiveType.SevenZip)
        {
            using var reader = archive.ExtractAllEntries();
            while (reader.MoveToNextEntry())
                action(new ReaderShim(reader.Entry, reader.OpenEntryStream));
        }
        else
        {
            foreach (var entry in archive.Entries)
                action(new ReaderShim(entry, entry.OpenEntryStream));
        }
    }

    /// <summary>Provides the subset of IReader used by the regular archive importer.</summary>
    public readonly record struct ReaderShim(IEntry Entry, Func<Stream> OpenEntryStream)
    {
        public void WriteEntryToDirectory(string directory)
        {
            var path = Path.Combine(directory, Entry.Key!);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using var entryStream = OpenEntryStream();
            using var fileStream  = File.Open(path, FileMode.Create, FileAccess.Write);
            entryStream.CopyTo(fileStream);
        }
    }
}
