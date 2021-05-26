package AST;
import Utilities.Visitor;

public class IfStat extends Statement {

	/** Note that elsepart() can return null! */

	public IfStat(Expression expr, Statement thenpart) {
		this(expr, thenpart, null);
	}

	public IfStat(Expression expr, Statement thenpart, Statement elsepart) {
		super(expr);
		nchildren = 3;
		children = new AST[] { expr, thenpart, elsepart };
	}

	public Expression expr()     { return (Expression)children[0]; }
	public Statement  thenpart() { return (Statement)children[1];  }
	public Statement  elsepart() { return (Statement)children[2];  }

	/* *********************************************************** */
	/* **                                                       ** */
	/* ** Generic Visitor Stuff                                 ** */
	/* **                                                       ** */
	/* *********************************************************** */

	public Object visit(Visitor v) {
		return v.visitIfStat(this);
	}


}
