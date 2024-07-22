namespace ITI_Material.IRepository
{
    public interface IServicesDepartements<T> where T : class
    {
        List<T> GetAll();
        T FindBy(int? Id);
        T FindBy(string Name);
        bool Save(T model);
        bool Delete(int Id);
    }
}
