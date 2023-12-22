namespace EmpathyKick.Data
{
    using EmpathyKick.Models;
    public static class MyDBContextExtensions
    {
        public static Organization[] getOrganizationsDonatedTo(this MyDBContext context, int id)
        {
            var organizations = context.Donation
              .Where(donation => donation.UserId == id)
              .Select(donation => donation.OrganizationId)
              .Distinct()
              .ToList();

            var organizationsData = context.Organization
                .Where(org => organizations.Contains(org.OrganizationId))
                .ToArray();

            return organizationsData;
        }

        public static bool IsEmpathyAdmin(this MyDBContext context, int id)
        {
            bool isAdmin = context.EmpathyAdmin.Where(admin => admin.UserID == id)
                .Where(admin => admin.AuthorizationDate != null)
                .Where(admin => admin.DeauthorizationDate == null).Any();
            return isAdmin;

        }

        public static OrganizationAdmin[] IsOrganizationAdmin(this MyDBContext context, int id)
        {
            var isOrgAdmin = context.OrganizationAdmin.Where(admin => admin.UserID == id)
                .Where(admin => admin.AuthorizationDate != null)
                .Where(admin => admin.DeauthorizationDate == null).ToArray();
            return isOrgAdmin;
        }

    }
}