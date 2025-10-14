using System.Security.Cryptography;

namespace Bug.BetterThanYesterday.Domain.PlanParticipants.Utils
{
    public static class CombineGuids
    {
        internal static Guid Combine(this Guid current, Guid other)
        {
            if(current == Guid.Empty || other == Guid.Empty)
                throw new ArgumentException("Apenas GUIDs válidos podem ser combinados");

            using var sha = SHA256.Create();
            var guids = new List<Guid>{current, other};
            var combinedBytes = guids.SelectMany(g => g.ToByteArray()).ToArray();
            var hash = sha.ComputeHash(combinedBytes);
            return new Guid(hash.Take(16).ToArray());
        }
    }
}