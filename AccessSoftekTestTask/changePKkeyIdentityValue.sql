declare
@orderIdOldId int, @orderIdNewId int
set @orderIdOldId = 1
set @orderIdNewId = 2

ALTER  TABLE OrderItems NOCHECK CONSTRAINT ALL

print 'updating orderItems'

update OrderItems
set OrderID = @orderIdNewId
where OrderID = @orderIdOldId

ALTER  TABLE OrderItems CHECK CONSTRAINT ALL



set identity_insert Orders ON

SELECT * 
INTO #tmpTable
FROM Orders
where orderId = @orderIdOldId

delete from Orders where orderId = @orderIdOldId

insert into Orders(OrderId)
select  @orderIdNewId
from #tmpTable

DROP TABLE #tmpTable 

set identity_insert Orders OFF