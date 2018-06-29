using Kotoha.Plugins;

namespace Kotoha
{
    internal interface IControlMapper2 : IControlMapper
    {
        object FindByRole<T>(Role role);

        object FindByRole(Role role);
    }
}