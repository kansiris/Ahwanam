USE [MaaAahwanam]
GO
/****** Object:  StoredProcedure [dbo].[sp_OrderDetails]    Script Date: 10/08/2016 04:16:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_OrderDetails](@OrderNo bigint)
as begin
select 
Vendormasters.Id,
Vendormasters.BusinessName,
Vendormasters.Description,
Vendormasters.Address,
Vendormasters.City,
Vendormasters.Landmark,
Vendormasters.ContactNumber,
Vendormasters.ServicType,
(case when Vendormasters.Id is not null then (select top 1 imagename from VendorImages where VendorImages.VendorMasterId=Vendormasters.Id) end) as image,
OrderDetails.TotalPrice,OrderDetails.UpdatedDate from Vendormasters inner join OrderDetails on Vendormasters.Id=OrderDetails.VendorId where OrderDetails.OrderNo=@OrderNo
end


