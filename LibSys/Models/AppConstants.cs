namespace LibSys.Models
{
    public static class UIColors
    {
        public static readonly Color Success = Color.MediumSeaGreen;
        public static readonly Color Error = Color.Crimson;
        public static readonly Color Warning = Color.Orange;
        public static readonly Color Info = Color.RoyalBlue;
        public static readonly Color TextMuted = Color.DimGray;
        public static readonly Color BackgroundAlt = Color.FromArgb(245, 246, 250);
        public static readonly Color ButtonSecondary = Color.LightGray;
        public static readonly Color White = Color.White;
    }

    public static class UIIcons
    {
        public const string Available = "🟢";
        public const string Loaned = "🟡";
        public const string Lost = "🔴";
        public const string WrittenOff = "⚫";
        public const string Overdue = "🔺";

        public const string ActionIssue = "📗";
        public const string ActionReturn = "↩️";
        public const string ActionLoss = "❌";

        public const string User = "👤";
        public const string Money = "💰";

        // Додаткові іконки
        public const string Search = "🔎";
        public const string Add = "➕";
        public const string Edit = "✏️";
        public const string Back = "⬅";
        public const string Copies = "📥";
        public const string Home = "🏠";
        public const string Settings = "⚙";
        public const string Catalog = "📚";
        public const string Inventory = "📦";
        public const string Journal = "🕒";
        public const string SuccessIcon = "✅";
        public const string ErrorIcon = "❌";
        public const string Return = "↩️";
        public const string WriteOff = "⚫";
        public const string NoCover = "🖼️";
    }

    public static class Fonts
    {
        public static readonly Font Regular14 = new Font("Segoe UI", 14F);
        public static readonly Font Bold14 = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static readonly Font Italic14 = new Font("Segoe UI", 14F, FontStyle.Italic);
        public static readonly Font Underline14 = new Font("Segoe UI", 14F, FontStyle.Underline);
        public static readonly Font Bold16 = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font Bold18 = new Font("Segoe UI", 18F, FontStyle.Bold);
        public static readonly Font Regular13 = new Font("Segoe UI", 13F);
    }
}