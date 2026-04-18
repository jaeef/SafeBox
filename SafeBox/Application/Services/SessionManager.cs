using SafeBox.Domain.Entities;

namespace SafeBox.Application.Services
{
    public static class SessionManager
    {
        private static User _currentUser;
        private static Admin _currentAdmin;

        public static User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                _currentAdmin = null;
            }
        }

        public static Admin CurrentAdmin
        {
            get { return _currentAdmin; }
            set
            {
                _currentAdmin = value;
                _currentUser = null;
            }
        }

        public static int CurrentUserId => _currentUser?.UserId ?? 0;

        public static int CurrentAdminId => _currentAdmin?.AdminId ?? 0;

        public static bool IsUserLoggedIn => _currentUser != null;

        public static bool IsAdminLoggedIn => _currentAdmin != null;

        public static void ClearSession()
        {
            _currentUser = null;
            _currentAdmin = null;
        }
    }
}
