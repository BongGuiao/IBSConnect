DROP VIEW IF EXISTS vw_billing;

CREATE VIEW vw_billing
AS
select
    MemberId,	
    IdNo,
    FirstName,
	MiddleName,
	LastName,
	AllottedTime,
	TotalMinutes,
    PaidMinutes,
	AllottedTime + PaidMinutes - TotalMinutes as TimeLeft,
	case 
		when TotalMinutes - PaidMinutes - AllottedTime > 0 then TotalMinutes - PaidMinutes - AllottedTime
		else 0 
	end as ExcessMinutes
from (
	select 
	m.Id AS MemberId,
    IdNo,
	FirstName,
	MiddleName,
	LastName,
	(select sum(mc.Credit) from member_credits mc where mc.MemberId = m.Id) as AllottedTime,
	COALESCE((select sum(mp.Minutes) from member_payments mp where mp.MemberId = m.Id), 0) as PaidMinutes,
	COALESCE(sum(TIMESTAMPDIFF(MINUTE, ms.StartTime, ms.EndTime)), 0) as TotalMinutes
	from members m
	inner join member_sessions ms on ms.MemberId = m.Id
	inner join categories c on m.CategoryId = c.Id
	where c.IsFreeTier = 0
	group by 
	m.Id,
    m.IdNo,
	m.FirstName,
	m.MiddleName,
	m.LastName,
	AllottedTime
) as billing