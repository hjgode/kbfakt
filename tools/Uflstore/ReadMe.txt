________________________________________________________________ 

Crystal Decisions Technical Support - uflstore.zip


Self-serve Support:
http://support.crystaldecisions.com/

Email Support:
http://support.crystaldecisions.com/support/answers.asp 

Telephone Support:
http://www.crystaldecisions.com/contact/support.asp

________________________________________________________________ 

PRODUCT VERSION

This User Functional Library DLL (UFL) is for use with version 5 or higher of Crystal Reports.

________________________________________________________________ 

DESCRIPTION

These DLLs add a number of Store and Fetch functions that allow you to pass string, numeric, date, currency and boolean variables 
back and forth between the Main Report and any number of Subreports.

________________________________________________________________ 


FILES

u25store.dll    32bit	 ...\winnt\crystal (for Win NT or Win 2000)
			 ...\windows\crystal (for Win 9x)

________________________________________________________________ 

INSTALLATION

1. To install on your own computer, copy the enclosed DLL file to your \WINDOWS\CRYSTAL or \WINNT\CRYSTAL folder.

The additional functions will be available the next time you start the Crystal Reports Designer.


2. To distribute this file for use with a compiled report or application, copy the file to your client's 
\WINDOWS\SYSTEM or WINNT\SYSTEM directory. 

________________________________________________________________ 

ADDITIONAL INFO / LIMITATIONS (optional)

Functions Added:
 ---------------------------
 StoreNumberVar(X, Y)
 StoreStringVar(X, Y)
 StoreDateVar(X, Y)
 StoreCurrencyVar(X, Y)
 StoreBooleanVar(X, Y)
 FetchNumberVar(X)
 FetchStringVar(X)
 FetchDateVar(X)
 FetchCurrencyVar(X)
 FetchBooleanVar(X)


 How to use these functions
 ---------------

To pass data from the subreport to the main report, create a 'store' formula in the subreport, and a 'fetch' formula in the main report.  To do the reverse, create the 'store' formula in the main report, and the 'fetch' formula in the subreport.

 1.  Create a store formula.

 Place the information into the memory variable 
 by using the store function that matches the type of information
 being stored.  For example, use StoreNumberVar(x,y) for numeric fields,
 and StoreStringVar(x,"y") for string fields - note x must be a string.

 {@Store_Example}

 //assigns the value 1000 to a numeric variable called 'SubreportTotal'.
 WhilePrintingRecords
 StoreNumberVar("SubreportTotal",1000)

 2.  Create a @Fetch Formula.

 The second step is to retrieve the information using the
 corresponding fetch function and variable name.

 {@Fetch_Example}

 //retrieves the variable 'SubreportTotal'
 WhilePrintingRecords
 FetchNumberVar("SubreportTotal")

 Tips 
 ---------------

 1. Use WhilePrintingRecords in all formulas that use the store and fetch functions.  This will make Crystal Reports evaluate the formulas during the same evaluation time.

 2. The formula that fetches the variable MUST NOT be in the same section that contains the subreport.  It must be in a section BELOW the subreport section.  This is to make sure that the subreport where a value is saved into the variable gets evaluated first than the formula that fetches the variable.

 3. If the subreport is suppressed or if the query of the subreport does not return any records, the formulas that store into the variable will not be evaluated.  This means the variable never gets created, and this will cause a "Variable not found" error if you try to fetch the variable.  To avoid this, create a formula in the main report that initializes the variable to zero, and insert this into a section above the section that contains the subreport.  This will cause the variable to be created even if the formula in the subreport is not evaluated.


 
 Syntax
 ---------
 StoreNumberVar(X, Y)
	Usage:	Stores a numeric value Y to X.  If a formula fields 
                contains this function, it will return the numeric
                value Y.
	Example:  StoreNumberVar("gTotal", 1000).
	Returns:  1000


 StoreStringVar(X, Y)
	Usage:	Stores a string value Y to key X.  If a formula fields
                just contains this function, it will return a string
                value Y.
	Example:  StoreStringVar("My Name", "Beanie")
	Returns:  Beanie

 StoreDateVar(X, Y)
	Usage:	Stores a date value Y to key X.  If a formula fields 
                just contains this function, it will return a date
                value Y.
	Example:  StoreDateVar("MyBDay", Date(1970,04,02))
	Returns:  April 2, 1970

 StoreCurrencyVar(X, Y)
	Usage:	Stores a string value Y to key X.  If a formula fields 
                just contains this function, it will return a string
                value Y.  This is same as using SaveNumberVar().
	Example:  StoreCurrencyVar("gTotal", 1000)
	Returns:  $1,000.00

 StoreBooleanVar(X, Y)
	Usage:	Stores a True/False value Y to key X.  If a formula
                fields just contains this function, it will return
                a string value Y.
	Example:  StoreBooleanVar("Gender Is Male", True)
	Returns:  True

 FetchNumberVar(X)
	Usage:  Given the key X, it will return a numeric 
                value which is stored under this key.  	
	Example: FetchNumberVar("gTotal")
	Returns: 1000

 FetchStringVar(X)
 	Usage:  Given the key X, it will return a string value
                which is stored under this key.  	
	Example: FetchStringVar("My Name")
	Returns: Beanie

 FetchDateVar(X)
	Usage:  Given the key X, it will return a date value
                which is stored under this key.  	
	Example: FetchDateVar("MyBDate")
	Returns: April 2, 1970

 FetchCurrencyVar(X)
	Usage:  Given the key X, it will return a currency value
                which is stored under this key.  	
	Example: FetchCurrencyVar("gTotal")
	Returns: $1,000.00

 FetchBooleanVar(X)
	Usage:  Given the key X, it will return a boolean value
                which is stored under this key.  	
	Example: FetchBooleanVar("Gender is Male")
	Returns: True

________________________________________________________________ 

Last updated on January 19, 2001
________________________________________________________________ 
