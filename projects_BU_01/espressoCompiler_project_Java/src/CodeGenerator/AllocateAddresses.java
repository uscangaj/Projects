package CodeGenerator;

import AST.*;
import Utilities.Visitor;



class AllocateAddresses extends Visitor {

    private Generator gen;
    private ClassDecl currentClass;
    
    AllocateAddresses(Generator g, ClassDecl currentClass, boolean debug) {
	this.debug = debug;
	gen = g;
	this.currentClass = currentClass;
    }
    
    
    
    // LOCAL VARIABLE DECLARATION
    public Object visitLocalDecl(LocalDecl ld) {	
	println(ld.line + ": LocalDecl:\tAssigning address:  " + ld.address + " to local variable '" + ld.var().name().getname() + "'.");
	
	
	// YOUR CODE HERE
        
	//set address
	ld.address = gen.getAddress();
        
	//increment address
	if (ld.type().isDoubleType() || ld.type().isLongType())
	    gen.inc2Address();
	else
	    gen.incAddress();
	
	
        return null;
    }
    
    
    
    // PARAMETER DECLARATION
    public Object visitParamDecl(ParamDecl pd) {
	println(pd.line + ": ParamDecl:\tAssigning address:  " + pd.address + " to parameter '" + pd.paramName().getname() + "'.");
	
	
	// YOUR CODE HERE
	
	//set address
	pd.address = gen.getAddress();
	//println("   param decl address: " + pd.address);
	
	
	//increment address
	if (pd.type().isDoubleType() || pd.type().isLongType())
	    gen.inc2Address();
	else
	    gen.incAddress();
        
	
        return null;
    }
    
    
    
    // METHOD DECLARATION
    public Object visitMethodDecl(MethodDecl md) {
	println(md.line + ": MethodDecl:\tResetting address counter for method '" + md.name().getname() + "'.");
	
	// YOUR CODE HERE
        
	//initialize address
	if (md.getModifiers().isStatic())
	    gen.setAddress(0);
	else
	    gen.setAddress(1);
        
	//visit children
	super.visitMethodDecl(md);
	
	//update localsUsed to gen.getLocalsUsed()
        md.localsUsed = gen.getAddress();
	
	
	println(md.line + ": End MethodDecl");	
	return null;
    }
    
    
    
    // CONSTRUCTOR DECLARATION
    public Object visitConstructorDecl(ConstructorDecl cd) {	
	println(cd.line + ": ConstructorDecl:\tResetting address counter for constructor '" + cd.name().getname() + "'.");
	
	//initialize address
        gen.setAddress(1);  //at address 0 is 'this'
        
	//visit children
	super.visitConstructorDecl(cd);
	
	//update localsUsed
        cd.localsUsed = gen.getAddress();
	

	println(cd.line + ": End ConstructorDecl");
	return null;
    }
    
    
    
    // STATIC INITIALIZER
    public Object visitStaticInitDecl(StaticInitDecl si) {
	println(si.line + ": StaticInit:\tResetting address counter for static initializer for class '" + currentClass.name() + "'.");
	
	// YOUR CODE HERE

	//initialize address
	gen.setAddress(0);  //bc we dont have 'this'
	
	//visit children
	super.visitStaticInitDecl(si);
	
	//update localsUsed
        si.localsUsed = gen.getAddress();
	
	
	println(si.line + ": End StaticInit");
	return null;
    }
}

