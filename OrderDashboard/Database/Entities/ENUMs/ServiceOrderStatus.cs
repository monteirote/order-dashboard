using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OrderDashboard.Database.Entities.ENUMs
{
    public enum ServiceOrderStatus
    {
        [Display(Name = "Em Andamento")]
        InProgress = 0,

        [Display(Name = "Concluída")]
        Completed = 1,

        [Display(Name = "Cancelada")]
        Cancelled = 2,
    }

    public static class EnumExtensions
    {
        public static string GetDisplayName (this Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.GetName() ?? enumValue.ToString();
        }
    }
}
