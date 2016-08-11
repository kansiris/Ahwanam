USE [MaaAahwanam]
GO
/****** Object:  StoredProcedure [dbo].[sp_AllOrders]    Script Date: 10/08/2016 03:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_AllOrders]
as begin
select data1.OrderId,data2.ServicType,data1.OrderDate,data1.TotalPrice,data1.Status  --data1=Orders,data2=Vendormasters
from Orders data1 
join Vendormasters data2 
on data1.OrderedBy=data2.Id
end