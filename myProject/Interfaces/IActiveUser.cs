using myProject.Models;

namespace myProject.Interfaces
{
    public interface IActiveUser
    {
        User? ActiveUser { get; }
    }
}