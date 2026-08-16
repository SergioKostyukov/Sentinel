using SentinelApi.Identity.Infrastructure.Security;
using Xunit;

namespace SentinelApi.Identity.Infrastructure.Tests;

public sealed class PasswordHasherTests
{
    private readonly PasswordHasher _sut = new();

    [Fact]
    public void Verify_WithCorrectPassword_ReturnsTrue()
    {
        var hash = _sut.Hash("correct-password");

        Assert.True(_sut.Verify(hash, "correct-password"));
    }

    [Fact]
    public void Verify_WithWrongPassword_ReturnsFalse()
    {
        var hash = _sut.Hash("correct-password");

        Assert.False(_sut.Verify(hash, "wrong-password"));
    }
}
