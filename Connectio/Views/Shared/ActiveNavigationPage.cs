using Microsoft.AspNetCore.Mvc.Rendering;

namespace Connectio.Views.Shared
{
    public static class ActiveNavigationPage
    {
        public static string Home => "Home";
        public static string Explore => "Explore";
        public static string Messages => "Messages";
        public static string Bookmarks => "Bookmarks";
        public static string Profile => "Profile";
        public static string Settings => "Settings";

        public static string HomeActivePage(ViewContext viewContext) => ActiveNavigationPageClass(viewContext, Home);
        public static string ExploreActivePage(ViewContext viewContext) => ActiveNavigationPageClass(viewContext, Explore);
        public static string MessagesActivePage(ViewContext viewContext) => ActiveNavigationPageClass(viewContext, Messages);
        public static string BookmarksActivePage(ViewContext viewContext) => ActiveNavigationPageClass(viewContext, Bookmarks);
        public static string ProfileActivePage(ViewContext viewContext) => ActiveNavigationPageClass(viewContext, Profile);
        public static string SettingsActivePage(ViewContext viewContext) => ActiveNavigationPageClass(viewContext, Settings);

        public static string ActiveNavigationPageClass(ViewContext viewContext, string page)
        {
            var activeController = viewContext.ViewData["ActiveNavigationPage"] as string;
            return string.Equals(activeController, page, StringComparison.OrdinalIgnoreCase) ? "font-bold text-orange-400" : string.Empty;
        }
    }
}
