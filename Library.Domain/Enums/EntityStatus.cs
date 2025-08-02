namespace Library.Domain.Enums;

public enum EntityStatus
{
    DELETED = 0,
    ACTIVE = 1,
}

public static class EnumRecordDeletedExtensions
{
    public static bool IsDeleted(this EntityStatus recordStatus)
    {
        return recordStatus == EntityStatus.DELETED;
    }

    public static bool IsActive(this EntityStatus recordStatus)
    {
        return recordStatus == EntityStatus.ACTIVE;
    }
}