using Contracts.Enums;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Application.Services;

public class SubscriptionExpiredChecker(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ISubscriptionExpiredChecker
{
    public async Task CheckAsync(CancellationToken cancellationToken)
    {
        var users = await userRepository
            .GetWithExpiredSubscriptionAsync(cancellationToken);

        foreach (var user in users)
        {
            user.SetSubscription(Guid.Parse(SubscriptionEnum.Free), 9999);

            await userRepository
                .UpdateAsync(user, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
