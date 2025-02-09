using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlSyntaxEnd { }

public interface ISqlSyntaxEnd<out TStatement> : ISqlSyntaxEnd
    where TStatement : SqlStatement { }
