using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace BC.AuthenticationMicroservice.CustomExceptions
{
    [Serializable]
    public class UserCreationException : Exception
    {
        public IEnumerable<IdentityError> IdentityErrors { get; }
        public UserCreationException() { }

        public UserCreationException(IEnumerable<IdentityError> identityErrors)
            : base("Error when creating a user: " + JsonSerializer.Serialize(identityErrors))
        {
            IdentityErrors = identityErrors;
        }
    } 

  
}