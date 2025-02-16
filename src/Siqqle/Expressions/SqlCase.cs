using System.Collections.Generic;

namespace Siqqle.Expressions;

public class SqlCase : SqlValue
{
    private List<SqlWhen> _when;

    public SqlCase() { }

    public SqlCase(SqlValue value)
    {
        Value = value;
    }

    public IReadOnlyCollection<SqlWhen> When => _when ?? (_when = []);

    public SqlValue Value { get; }

    public SqlElse Else { get; internal set; }

    public string Alias { get; internal set; }

    public override SqlExpressionType ExpressionType => SqlExpressionType.Case;

    internal void AddWhen(SqlWhen when)
    {
        if (_when == null)
        {
            _when = [];
        }
        _when.Add(when);
    }
}
