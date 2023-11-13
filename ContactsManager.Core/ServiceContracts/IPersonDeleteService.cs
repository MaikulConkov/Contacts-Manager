namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonDeleteService
    {
        Task<bool> DeletePerson(Guid? personID);

    }
}
