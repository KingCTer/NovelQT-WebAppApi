namespace NovelQT.Domain.Models.Enum
{
    public enum IndexStatusEnum
    {
        /// <summary>
        /// The document has no Status assigned.
        /// </summary>
        None = 0,

        /// <summary>
        /// The document is scheduled for indexing by a BackgroundService.
        /// </summary>
        ScheduledIndex = 1,

        /// <summary>
        /// The document is scheduled for deletion by a BackgroundService.
        /// </summary>
        ScheduledDelete = 2,

        /// <summary>
        /// The document has been indexed.
        /// </summary>
        Indexed = 3,

        /// <summary>
        /// The document has been indexed.
        /// </summary>
        IndexedInCloud = 4,

        /// <summary>
        /// The document has been indexed.
        /// </summary>
        IndexedInDocker = 5,

        /// <summary>
        /// The document indexing has failed due to an error.
        /// </summary>
        Failed = 6,

        /// <summary>
        /// The document has been removed from the index.
        /// </summary>
        Deleted = 7,

        /// <summary>
        /// The document has been removed from the index.
        /// </summary>
        DeletedInCloud = 8,

        /// <summary>
        /// The document has been removed from the index.
        /// </summary>
        DeletedInDocker = 9
    }
}
