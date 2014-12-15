表达式优先级(从高到低）：
ValueExpression
VarExpression
FuncExpression
()
!
CompareExpression
LogicExpression
*/%
+-
-----------------------
逻辑表达式用例：
and 	1&1=1	1&0=0	0&1=0	0&0=0
or	    1|1=1	1|0=1	0|1=1	0|0=0
xor	    1^1=0	1^0=1	0^1=1	0^0=0

