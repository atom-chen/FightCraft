--ս����
x100004_g_scriptId = 100004

--�¼�����
function x100004_OnEventFilter(uuid, achEventId, fightvalue)

	if fightvalue ~= nil and tonumber(fightvalue) <= LuaGetLineupFightValue(uuid) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100004_OnAchievementProgress(uuid, reachNum)
  	return reachNum
end

