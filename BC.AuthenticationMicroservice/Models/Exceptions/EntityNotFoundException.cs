namespace BC.AuthenticationMicroservice.Models.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, string id)
            : base($"Entity {entityName} with id/name = {id} cannot be found!")
        {

        }
    }
}
