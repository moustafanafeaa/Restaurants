
namespace Restaurants.Domain.Exeptions
{
    public class NotFoundException(string resourceType, string resourceIdentifier) : Exception($"{resourceType} with id: {resourceIdentifier} dosen't exist")
    {

    }
}
