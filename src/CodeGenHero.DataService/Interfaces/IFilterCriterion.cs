namespace CodeGenHero.DataService
{
    public interface IFilterCriterion
    {
        string FieldName { get; set; }
        string FilterOperator { get; set; }
        object Value { get; set; }
    }
}