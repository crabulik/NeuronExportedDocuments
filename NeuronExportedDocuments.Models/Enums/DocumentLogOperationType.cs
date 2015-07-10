namespace NeuronExportedDocuments.Models.Enums
{
    public enum DocumentLogOperationType
    {
        Imported = 1,
        Published = 2,
        InfoSentedToUser = 3,
        SuccessAccess = 4,
        FailAccess = 5,
        Blocked = 6,
        SetToArchive = 7,
        UnblockedByAdmin = 8,
        Republished = 9,
        Archived = 10
    }
}