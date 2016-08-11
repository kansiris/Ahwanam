USE [MaaAahwanam]
GO
/****** Object:  StoredProcedure [dbo].[sp_LeastBidRecord]    Script Date: 10/08/2016 05:06:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_LeastBidRecord](@RequestId bigint)
as begin
select data2.ResponseId,data1.EventName,data1.EventAddress,data1.EventLocation,data1.PhoneNo,data1.EventStartDate,data1.EventStartTime,data2.Amount,data1.Budget 
from ServiceRequests data1 
join ServiceResponses data2 
on data1.RequestId=data2.RequestId
where data2.RequestId=@RequestId and data2.Amount<data1.Budget and data2.Amount = (select min(Amount) from ServiceResponses where RequestId=@RequestId)
end