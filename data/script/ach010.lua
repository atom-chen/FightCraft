--�ż���ʯ
x100010_g_scriptId = 100010

--�¼�����
function x100010_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 9) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100010_OnAchievementProgress(uuid, reachNum)
  	return 1
end

