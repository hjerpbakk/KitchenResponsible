namespace KitchenResponsible.Controllers.Repositories
{
    public interface IEmployeeService
    {
        ResponsibleForWeek GetEmployeeForWeek(ushort week);
    }
}